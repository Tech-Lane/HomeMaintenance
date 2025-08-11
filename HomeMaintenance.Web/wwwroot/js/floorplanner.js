let planners = {};

function getCtx(id){
  const c=document.getElementById(id); if(!c) return null; return c.getContext('2d');
}

export function initFloorPlanner(id, options, initialJson, dotnetRef){
  const ctx=getCtx(id); if(!ctx) return;
  planners[id]={ ctx, options: options||{}, objects: [], state:{ snap:true, units: options?.units||'imperial', measure:false, hover:null, sel:null, heat:false, layer:'All', drag:null, poly:null, view:{ scale:1, ox:0, oy:0 }, panning:null }, dotnetRef };
  draw(id);
}

export function setUnits(id, units){ const p=planners[id]; if(!p) return; p.state.units=units; draw(id);} 
export function addRoomRect(id){ const p=planners[id]; if(!p) return; p.objects.push({type:'room', x:40, y:40, w:200, h:150, angle:0, name:'Room', color:'#dbeafe'}); draw(id);} 
export function addFurniture(id){ const p=planners[id]; if(!p) return; p.objects.push({type:'furniture', x:80, y:80, w:80, h:40, angle:0, name:'Sofa', color:'#fde68a'}); draw(id);} 
export function addCustom(id){ const p=planners[id]; if(!p) return; p.objects.push({type:'custom', x:120, y:120, w:60, h:60, angle:0, name:'Custom', color:'#e9d5ff'}); draw(id);} 
export function addDoor(id){ const p=planners[id]; if(!p) return; p.objects.push({type:'door', x:50, y:50, w:40, h:10, angle:0, name:'Door', color:'#cbd5e1'}); draw(id);} 
export function addWindow(id){ const p=planners[id]; if(!p) return; p.objects.push({type:'window', x:70, y:70, w:60, h:8, angle:0, name:'Window', color:'#bae6fd'}); draw(id);} 
export function addMarker(id, kind){ const p=planners[id]; if(!p) return; p.objects.push({type:'marker', kind, x:160, y:160}); draw(id);} 
export function enableMeasure(id){ const p=planners[id]; if(!p) return; p.state.measure=!p.state.measure; }
export function toggleSnap(id){ const p=planners[id]; if(!p) return; p.state.snap=!p.state.snap; }
export function deleteSelected(id){ const p=planners[id]; if(!p) return; if(p.state.sel!=null){ p.objects.splice(p.state.sel,1); p.state.sel=null; draw(id);} }
export function toggleHeatmap(id){ const p=planners[id]; if(!p) return; p.state.heat=!p.state.heat; draw(id);}
export function setLayer(id, layer){ const p=planners[id]; if(!p) return; p.state.layer=layer; draw(id);}
export function updateSelected(id, payload){ const p=planners[id]; if(!p) return; const i=p.state.sel; if(i==null) return; const o=p.objects[i]; Object.assign(o, {name:payload.name, x:payload.x, y:payload.y, w:payload.w, h:payload.h, layer:payload.layer}); draw(id); }

export function exportPlan(id){ const p=planners[id]; if(!p) return '{}'; return JSON.stringify({options:p.options, objects:p.objects}, null, 2);} 
export function downloadJson(text, filename){ const a=document.createElement('a'); a.href=URL.createObjectURL(new Blob([text],{type:'application/json'})); a.download=filename; a.click(); }
export function loadPlan(id, json){ const p=planners[id]; if(!p) return; try{ const data=JSON.parse(json); if(Array.isArray(data.objects)) p.objects=data.objects; draw(id);}catch{} }

function draw(id){
  const p=planners[id]; if(!p) return; const ctx=p.ctx; const c=ctx.canvas;
  // Reset transform and clear
  ctx.setTransform(1,0,0,1,0,0);
  ctx.clearRect(0,0,c.width,c.height);
  // Resize canvas to element size
  const rect=c.getBoundingClientRect(); c.width=rect.width; c.height=rect.height;
  // Apply pan/zoom
  ctx.setTransform(p.state.view.scale, 0, 0, p.state.view.scale, p.state.view.ox, p.state.view.oy);
  // grid
  const grid=p.options.grid?.size||24; ctx.strokeStyle=p.options.grid?.color||'#eee'; ctx.lineWidth=1;
  for(let x=0;x<c.width;x+=grid){ ctx.beginPath(); ctx.moveTo(x,0); ctx.lineTo(x,c.height); ctx.stroke(); }
  for(let y=0;y<c.height;y+=grid){ ctx.beginPath(); ctx.moveTo(0,y); ctx.lineTo(c.width,y); ctx.stroke(); }
  // objects
  p.objects.forEach((o,i)=>{
    if(p.state.layer!=='All' && o.layer && o.layer!==p.state.layer) return;
    if(o.type==='polyroom'){
      const pts=o.points||[]; if(pts.length>1){
        ctx.save(); ctx.fillStyle=o.color||'rgba(219,234,254,0.6)'; ctx.strokeStyle='#64748b';
        ctx.beginPath(); ctx.moveTo(pts[0].x, pts[0].y); for(let k=1;k<pts.length;k++){ ctx.lineTo(pts[k].x, pts[k].y); } ctx.closePath(); ctx.fill(); ctx.stroke(); ctx.restore();
      }
      return;
    }
    if(o.type==='room' || o.type==='furniture' || o.type==='custom'){
      const ang=(o.angle||0)*Math.PI/180; const cx=o.x+o.w/2, cy=o.y+o.h/2;
      ctx.save(); ctx.translate(cx,cy); if(ang) ctx.rotate(ang);
      ctx.fillStyle=o.color||'#ddd'; ctx.fillRect(-o.w/2,-o.h/2,o.w,o.h);
      ctx.strokeStyle='#777'; ctx.strokeRect(-o.w/2,-o.h/2,o.w,o.h);
      ctx.restore();
      if(o.name){ ctx.fillStyle='#333'; ctx.font='12px system-ui'; ctx.fillText(o.name, o.x+6, o.y+16); }
    } else if(o.type==='door' || o.type==='window'){
      const ang=(o.angle||0)*Math.PI/180; const cx=o.x+o.w/2, cy=o.y+o.h/2;
      ctx.save(); ctx.translate(cx,cy); if(ang) ctx.rotate(ang);
      ctx.fillStyle=o.color||'#ddd'; ctx.fillRect(-o.w/2,-o.h/2,o.w,o.h);
      ctx.strokeStyle=o.type==='door'?'#64748b':'#0ea5e9'; ctx.strokeRect(-o.w/2,-o.h/2,o.w,o.h);
      ctx.restore();
      if(o.name){ ctx.fillStyle='#333'; ctx.font='11px system-ui'; ctx.fillText(o.name, o.x+4, o.y-4); }
    } else if(o.type==='marker'){
      ctx.fillStyle='#0ea5e9'; ctx.beginPath(); ctx.arc(o.x,o.y,6,0,Math.PI*2); ctx.fill();
      ctx.fillStyle='#333'; ctx.font='10px system-ui'; ctx.fillText(o.kind||'marker', o.x+8, o.y+4);
    }
  });
  // selection outline & handles
  if(p.state.sel!=null){
    const o=p.objects[p.state.sel];
    if(o){
      ctx.save();
      ctx.setLineDash([6,4]);
      ctx.strokeStyle='#2563eb';
      if(o.type==='marker'){
        ctx.beginPath(); ctx.arc(o.x,o.y,10,0,Math.PI*2); ctx.stroke();
      } else {
        // draw selection box using axis-aligned bounds even if rotated
        const bbox = getBBox(o);
        ctx.strokeRect(bbox.x-2,bbox.y-2,bbox.w+4,bbox.h+4);
        // handles
        const hs=6; const handles=getHandles(o);
        ctx.setLineDash([]); ctx.fillStyle='#2563eb'; ctx.strokeStyle='#ffffff';
        handles.forEach(h=>{ ctx.fillRect(h.x-hs/2,h.y-hs/2,hs,hs); ctx.strokeRect(h.x-hs/2,h.y-hs/2,hs,hs); });
      }
      ctx.restore();
    }
  }
  if(p.state.heat){
    // naive heat overlay around wifi markers
    p.objects.filter(o=>o.type==='marker' && o.kind==='wifi').forEach(o=>{
      const grd=ctx.createRadialGradient(o.x,o.y,10,o.x,o.y,120);
      grd.addColorStop(0,'rgba(14,165,233,0.35)');
      grd.addColorStop(1,'rgba(14,165,233,0)');
      ctx.fillStyle=grd; ctx.beginPath(); ctx.arc(o.x,o.y,120,0,Math.PI*2); ctx.fill();
    });
  }
  // draw in-progress polygon
  if(p.state.poly && p.state.poly.points.length){
    const pts=p.state.poly.points; ctx.save(); ctx.strokeStyle='#2563eb'; ctx.setLineDash([4,3]);
    ctx.beginPath(); ctx.moveTo(pts[0].x, pts[0].y); for(let k=1;k<pts.length;k++){ ctx.lineTo(pts[k].x, pts[k].y); } ctx.stroke(); ctx.restore();
  }
}

// Selection stub: click to select nearest object
document.addEventListener('click', e=>{
  for(const id in planners){
    const p=planners[id]; const c=p.ctx.canvas; const r=c.getBoundingClientRect();
    if(e.clientX>=r.left && e.clientX<=r.right && e.clientY>=r.top && e.clientY<=r.bottom){
      const wp = screenToWorld(p, e.clientX-r.left, e.clientY-r.top); const x=wp.x, y=wp.y;
      // poly mode: add point
      if(p.state.poly){ p.state.poly.points.push(snapPt(p,{x,y})); draw(id); return; }
      let idx=null; for(let i=p.objects.length-1;i>=0;i--){ const o=p.objects[i]; if(o.type==='marker'){ const dx=x-o.x, dy=y-o.y; if(dx*dx+dy*dy<100){ idx=i; break;} } else { if(x>=o.x && x<=o.x+o.w && y>=o.y && y<=o.y+o.h){ idx=i; break;} } }
      p.state.sel=idx; draw(id);
      if(p.dotnetRef){ const o=idx!=null?p.objects[idx]:null; const payload=o? JSON.stringify({ type:o.type, name:o.name||'', x:o.x||0, y:o.y||0, w:o.w||0, h:o.h||0, layer:o.layer||'All' }): null; p.dotnetRef.invokeMethodAsync('OnSelectionChanged', payload); }
      break;
    }
  }
});

// Simple measure feedback: on mousemove when measure is on, show mouse pos in current units
document.addEventListener('mousemove', e=>{
  for(const id in planners){
    const p=planners[id]; const c=p.ctx.canvas; const r=c.getBoundingClientRect();
    if(e.clientX<r.left || e.clientX>r.right || e.clientY<r.top || e.clientY>r.bottom) continue;
    const x=e.clientX-r.left, y=e.clientY-r.top;
    // drag/resize
    if(p.state.drag){
      const o=p.objects[p.state.sel]; if(!o) continue;
      if(p.state.drag.mode==='move'){
        let nx = p.state.drag.offX? x - p.state.drag.offX : x; let ny = p.state.drag.offY? y - p.state.drag.offY : y;
        if(o.type==='marker'){ o.x = nx; o.y = ny; }
        else { o.x = nx - p.state.drag.dx; o.y = ny - p.state.drag.dy; }
        if(o.type==='door' || o.type==='window'){
          // Try to snap door/window to nearest polyroom wall; fall back to grid snap
          if(!snapDoorWindow(p, o) && p.state.snap){ const g=p.options.grid?.size||24; o.x=Math.round(o.x/g)*g; o.y=Math.round(o.y/g)*g; }
        } else if(p.state.snap){ const g=p.options.grid?.size||24; if(o.type==='marker'){ o.x = Math.round(o.x/g)*g; o.y=Math.round(o.y/g)*g; } else { o.x=Math.round(o.x/g)*g; o.y=Math.round(o.y/g)*g; } }
      } else if(p.state.drag.mode==='resize'){
        resizeWithHandle(o, p.state.drag.handle, x, y, p);
      }
      draw(id);
      continue;
    }
    if(p.state.poly){
      // preview last segment to cursor
      draw(id); // redraw base
      const pts=p.state.poly.points; if(pts.length){ const ctx=p.ctx; ctx.save(); ctx.strokeStyle='#2563eb'; ctx.setLineDash([4,2]); ctx.beginPath(); ctx.moveTo(pts[pts.length-1].x, pts[pts.length-1].y); const sp=snapPt(p,{x,y}); ctx.lineTo(sp.x,sp.y); ctx.stroke(); ctx.restore(); }
      continue;
    }
    if(p.state.measure){
      const text = p.state.units==='imperial' ? `x: ${(x/12).toFixed(1)} ft, y: ${(y/12).toFixed(1)} ft` : `x: ${(x/100).toFixed(2)} m, y: ${(y/100).toFixed(2)} m`;
      if(p.dotnetRef) p.dotnetRef.invokeMethodAsync('OnMeasureChanged', text);
    }
  }
});

document.addEventListener('mousedown', e=>{
  for(const id in planners){
    const p=planners[id]; const c=p.ctx.canvas; const r=c.getBoundingClientRect();
    if(e.clientX<r.left || e.clientX>r.right || e.clientY<r.top || e.clientY>r.bottom) continue;
    if(p.state.sel==null) continue;
    const o=p.objects[p.state.sel]; if(!o) continue;
    const x=e.clientX-r.left, y=e.clientY-r.top;
    if(o.type==='marker'){
      p.state.drag={ mode:'move', offX:0, offY:0, dx:0, dy:0 };
    } else {
      const hIndex=hitHandle(o,x,y);
      if(hIndex>=0){ p.state.drag={ mode:'resize', handle:hIndex }; }
      else { p.state.drag={ mode:'move', dx:x-o.x, dy:y-o.y } }
    }
    e.preventDefault();
    break;
  }
});

document.addEventListener('mouseup', e=>{
  for(const id in planners){
    const p=planners[id]; if(!p.state.drag) continue; p.state.drag=null; draw(id);
    if(p.dotnetRef && p.state.sel!=null){ const o=p.objects[p.state.sel]; const payload=o? JSON.stringify({ type:o.type, name:o.name||'', x:o.x||0, y:o.y||0, w:o.w||0, h:o.h||0, layer:o.layer||'All' }): null; p.dotnetRef.invokeMethodAsync('OnSelectionChanged', payload); }
  }
});

// Keyboard: arrows to nudge; shift+arrows for larger steps; R/E to rotate +/-5 deg
document.addEventListener('keydown', e=>{
  for(const id in planners){
    const p=planners[id]; if(p.state.sel==null) continue; const o=p.objects[p.state.sel]; if(!o) continue;
    const base = (e.shiftKey? (p.options.grid?.size||24) : 1);
    let changed=false;
    if(e.key==='ArrowLeft'){ if(o.type==='marker'){ o.x-=base; } else { o.x-=base; } changed=true; }
    if(e.key==='ArrowRight'){ if(o.type==='marker'){ o.x+=base; } else { o.x+=base; } changed=true; }
    if(e.key==='ArrowUp'){ if(o.type==='marker'){ o.y-=base; } else { o.y-=base; } changed=true; }
    if(e.key==='ArrowDown'){ if(o.type==='marker'){ o.y+=base; } else { o.y+=base; } changed=true; }
    if(e.key==='r' || e.key==='R'){ if(o.type!=='marker'){ o.angle=(o.angle||0)+5; changed=true; } }
    if(e.key==='e' || e.key==='E'){ if(o.type!=='marker'){ o.angle=(o.angle||0)-5; changed=true; } }
    if(changed){
      if(o.type==='door' || o.type==='window') snapDoorWindow(p, o);
      e.preventDefault(); draw(id); if(p.dotnetRef){ const payload=JSON.stringify({ type:o.type, name:o.name||'', x:o.x||0, y:o.y||0, w:o.w||0, h:o.h||0, layer:o.layer||'All' }); p.dotnetRef.invokeMethodAsync('OnSelectionChanged', payload);} break; }
  }
});

function getHandles(o){
  const b=getBBox(o);
  const points=[
    {x:b.x, y:b.y}, // tl
    {x:b.x+b.w/2, y:b.y}, // tm
    {x:b.x+b.w, y:b.y}, // tr
    {x:b.x, y:b.y+b.h/2}, // ml
    {x:b.x+b.w, y:b.y+b.h/2}, // mr
    {x:b.x, y:b.y+b.h}, // bl
    {x:b.x+b.w/2, y:b.y+b.h}, // bm
    {x:b.x+b.w, y:b.y+b.h} // br
  ];
  return points;
}

function hitHandle(o,x,y){
  const hs=8; const handles=getHandles(o);
  for(let i=0;i<handles.length;i++){ const h=handles[i]; if(Math.abs(x-h.x)<=hs && Math.abs(y-h.y)<=hs) return i; }
  return -1;
}

function resizeWithHandle(o, index, x, y, p){
  const g=p.options.grid?.size||24; const snap=(v)=> p.state.snap? Math.round(v/g)*g : v;
  let nx=x, ny=y; nx=snap(nx); ny=snap(ny);
  const b=getBBox(o); let left=b.x, top=b.y, right=b.x+b.w, bottom=b.y+b.h;
  let nleft=left, ntop=top, nright=right, nbottom=bottom;
  switch(index){
    case 0: nleft=nx; ntop=ny; break; // tl
    case 1: ntop=ny; break; // tm
    case 2: nright=nx; ntop=ny; break; // tr
    case 3: nleft=nx; break; // ml
    case 4: nright=nx; break; // mr
    case 5: nleft=nx; nbottom=ny; break; // bl
    case 6: nbottom=ny; break; // bm
    case 7: nright=nx; nbottom=ny; break; // br
  }
  // normalize ensure min size
  const min=10;
  let nw=Math.max(min, nright-nleft), nh=Math.max(min, nbottom-ntop);
  let nx0=Math.min(nleft, nright), ny0=Math.min(ntop, nbottom);
  // apply to unrotated box (approximate)
  o.x=nx0; o.y=ny0; o.w=nw; o.h=nh;
}

function getBBox(o){
  const ang=(o.angle||0)*Math.PI/180; if(!ang) return {x:o.x, y:o.y, w:o.w, h:o.h};
  const cx=o.x+o.w/2, cy=o.y+o.h/2;
  const corners=[
    {x:o.x, y:o.y}, {x:o.x+o.w, y:o.y}, {x:o.x, y:o.y+o.h}, {x:o.x+o.w, y:o.y+o.h}
  ].map(pt=>rotatePoint(pt,cx,cy,ang));
  const xs=corners.map(p=>p.x), ys=corners.map(p=>p.y);
  const minx=Math.min(...xs), maxx=Math.max(...xs), miny=Math.min(...ys), maxy=Math.max(...ys);
  return {x:minx, y:miny, w:maxx-minx, h:maxy-miny};
}

function rotatePoint(pt,cx,cy,ang){
  const dx=pt.x-cx, dy=pt.y-cy; return {x: cx + dx*Math.cos(ang)-dy*Math.sin(ang), y: cy + dx*Math.sin(ang)+dy*Math.cos(ang)};
}

// polygon room helpers
export function startPolyRoom(id){ const p=planners[id]; if(!p) return; p.state.poly={ points: [] }; }
export function finishPolyRoom(id){ const p=planners[id]; if(!p||!p.state.poly) return; const pts=p.state.poly.points; if(pts.length>=3){ p.objects.push({type:'polyroom', points: pts.slice(), name:'Room', color:'rgba(219,234,254,0.6)'}); } p.state.poly=null; draw(id);} 
export function cancelPolyRoom(id){ const p=planners[id]; if(!p) return; p.state.poly=null; draw(id); }
function snapPt(p,pt){ if(!p.state.snap) return pt; const g=p.options.grid?.size||24; return { x: Math.round(pt.x/g)*g, y: Math.round(pt.y/g)*g } }
function dist(x1,y1,x2,y2){ const dx=x2-x1, dy=y2-y1; return Math.sqrt(dx*dx+dy*dy);} 

// Snap door/window center to nearest polyroom wall segment
function snapDoorWindow(p, o){
  const center = { x: o.x + o.w/2, y: o.y + o.h/2 };
  let best = null;
  for(const obj of p.objects){
    if(obj.type !== 'polyroom') continue;
    const pts = obj.points || [];
    for(let i=0;i<pts.length;i++){
      const a = pts[i], b = pts[(i+1)%pts.length];
      const proj = projectPointOnSegment(center.x, center.y, a.x, a.y, b.x, b.y);
      const d = dist(center.x, center.y, proj.x, proj.y);
      if(best == null || d < best.d){ best = { d, proj, a, b } }
    }
  }
  if(!best) return false;
  const threshold = 40; // px
  if(best.d > threshold) return false;
  // align along wall with small normal offset (half thickness)
  const ang = Math.atan2(best.b.y - best.a.y, best.b.x - best.a.x);
  const nx = -Math.sin(ang), ny = Math.cos(ang);
  const offset = (o.h/2)||0;
  const cx = best.proj.x + nx*offset;
  const cy = best.proj.y + ny*offset;
  o.x = cx - o.w/2; o.y = cy - o.h/2; o.angle = ang * 180/Math.PI;
  return true;
}

function projectPointOnSegment(px, py, ax, ay, bx, by){
  const abx = bx - ax, aby = by - ay; const apx = px - ax, apy = py - ay; const ab2 = abx*abx + aby*aby; if(ab2 === 0) return { x: ax, y: ay };
  let t = (apx*abx + apy*aby)/ab2; t = Math.max(0, Math.min(1, t));
  return { x: ax + abx*t, y: ay + aby*t };
}
