// #465: So chu ky PROXY (Web References) voi chu ky WebMethod THAT (WSCarSv.asmx.cs).
// Lech so tham so => proxy gui thua/thieu phan tu SOAP => du lieu bi vut im lang.
const fs=require('fs');
const PROXY=process.argv[2], WS=process.argv[3];

function readSigs(file, isProxy){
  const src=fs.readFileSync(file,'utf8').split(/\r?\n/);
  console.log('  doc ' + file.split('/').pop() + ': ' + src.length + ' dong');
  const out=new Map();
  const head = isProxy
    ? /^\s*public\s+System\.Data\.DataSet\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/
    : /^\s*public\s+DataSet\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
  for(let i=0;i<src.length;i++){
    const L=src[i];
    if(/^\s*\/\//.test(L)) continue;                 // dong comment => bo (luat: port dong ACTIVE)
    const m=L.match(head); if(!m) continue;
    if(!isProxy){
      // Chi lay ham co [WebMethod] khong bi comment o ngay tren
      let j=i-1, ok=false;
      while(j>=0 && src[j].trim()==='') j--;
      if(j>=0 && /^\s*\[WebMethod/.test(src[j])) ok=true;
      if(!ok) continue;
    }
    // Dem tham so: gom tu dong nay den dau ')' o muc 0
    let depth=0, buf='', k=i, started=false;
    while(k<src.length && k<i+400){
      const line=src[k].replace(/\/\/.*$/,'');
      for(const ch of line){
        if(ch==='('){ depth++; started=true; if(depth===1) continue; }
        if(ch===')'){ depth--; if(depth===0){ k=src.length; break; } }
        if(started&&depth>=1) buf+=ch;
      }
      if(k===src.length) break;
      buf+=' '; k++;
    }
    const params=buf.split(',').map(x=>x.trim()).filter(x=>x.length>0);
    const names=params.map(p=>p.split(/\s+/).pop());
    if(!out.has(m[1])) out.set(m[1],{line:i+1,n:params.length,names});
  }
  return out;
}

const proxy=readSigs(PROXY,true), ws=readSigs(WS,false);
console.log('proxy=' + proxy.size + ' webmethod=' + ws.size);
let both=0, mismatch=[];
for(const [name,p] of proxy){
  const w=ws.get(name); if(!w) continue;
  both++;
  // proxy khong co strSessionId? WS co. So theo TEN de chac.
  const pn=new Set(p.names), wn=new Set(w.names);
  const onlyProxy=[...pn].filter(x=>!wn.has(x));
  const onlyWs=[...wn].filter(x=>!pn.has(x));
  if(onlyProxy.length||onlyWs.length)
    mismatch.push({name,pl:p.line,wl:w.line,pn:p.n,wn:w.n,onlyProxy,onlyWs});
}
console.log('ten khop hai ben=' + both + '  LECH=' + mismatch.length);
mismatch.sort((a,b)=>(b.onlyProxy.length+b.onlyWs.length)-(a.onlyProxy.length+a.onlyWs.length));
for(const m of mismatch.slice(0,25))
  console.log('  ' + m.name + ' proxy:' + m.pn + ' ws:' + m.wn
    + '  chiProxy=[' + m.onlyProxy.join(',') + ']  chiWS=[' + m.onlyWs.join(',') + ']');
