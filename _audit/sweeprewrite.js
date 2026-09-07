// #484: Trong cac cap _WH con "lech", bao nhieu cap chi la VIET LAI
// (ban Main dung BANG TAM + group by, ban _WH dung TRUY VAN CON TUONG QUAN)?
const fs=require('fs'),path=require('path');
const BIZ=process.argv[2], WS=process.argv[3];
function wsLiveNames(wsArg){
  const dirs=String(wsArg).split(',').map(x=>x.trim()).filter(Boolean);
  const out=new Set(); let files=0;
  const callRe=/_biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
  for(const d of dirs){ const st=[d];
    while(st.length){ const cur=st.pop();
      for(const e of fs.readdirSync(cur,{withFileTypes:true})){
        const fp=path.join(cur,e.name);
        if(e.isDirectory()){ if(!/^(bin|obj)$/.test(e.name)) st.push(fp); }
        else if(e.name.endsWith('.cs') && !/\.20\d{6}\.cs$/.test(e.name)){
          files++;
          for(const L of fs.readFileSync(fp,'utf8').split(/\r?\n/)){
            if(/^\s*\/\//.test(L)) continue;
            const m=L.match(callRe); if(m) out.add(m[1]);
          }
        }
      }
    }
  }
  console.log('  doc '+files+' file WS; ten biz duoc goi = '+out.size);
  return out;
}
function walk(d,a){for(const e of fs.readdirSync(d,{withFileTypes:true})){const fp=path.join(d,e.name);
 if(e.isDirectory()){if(!/^(bin|obj|Properties|Web References|Service References)$/.test(e.name))walk(fp,a);}
 else if(e.name.endsWith('.cs'))a.push(fp);}return a;}
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const files=walk(BIZ,[]);
console.log('  doc '+files.length+' file biz .cs');
const bodies=new Map();
for(const f of files){
  const lines=fs.readFileSync(f,'utf8').split(/\r?\n/); const mem=[];
  lines.forEach((L,i)=>{const m=L.match(memberRe); if(m&&!/^\s*\/\//.test(L)) mem.push({i,name:m[1]});});
  for(let k=0;k<mem.length;k++){
    const from=mem[k].i, to=(k+1<mem.length?mem[k+1].i:lines.length);
    if(!bodies.has(mem[k].name)) bodies.set(mem[k].name, lines.slice(from,to));
  }
}
const live=wsLiveNames(WS);
function macroCount(l){ return l.filter(x=>/zzB_[A-Za-z0-9_]+_zzE/.test(x)).length; }
function sqlOf(lines){
  return lines.map(x=>x.replace(/\r/g,''))
    .filter(x=>!/^\s*\/\/\//.test(x))
    .filter(x=>/select|from\s|where|join|group by|order by|into\s/i.test(x))
    .map(x=>x.replace(/\[@strDBName_CommonCenter\]\.\[dbo\]\./g,'')
             .replace(/_WH\b/g,'').replace(/_New\d{6,8}/g,'')
             .replace(/with\s*\(\s*nolock\s*\)/ig,'--//[mylock]')
             .replace(/--(?!\/\/\[mylock\]).*$/,'')
             .replace(/\(\s*1\s*=\s*1\s*\)/g,'1=1')
             .replace(/\s+/g,' ').trim().toLowerCase())
    .filter(x=>x.length>0);
}
const names=[...bodies.keys()];
const esc=x=>x.replace(/[.*+?^${}()|[\]\\]/g,'\\$&');
function pick(c){ const p=c.filter(n=>live.has(n)); if(!p.length) return null;
  p.sort((x,y)=>sqlOf(bodies.get(y)).length-sqlOf(bodies.get(x)).length); return p[0]; }
const bases=new Set();
for(const n of names){ const m=n.match(/^(.*)_WH(_New\d{6,8})?$/); if(m) bases.add(m[1]); }

// Dau hieu VIET LAI: ben Main co bang tam + group by; ben _WH co truy van con tuong quan
const tempRe=/into\s+#|group by/i;
const corrRe=/\(\s*select\s+(sum|count|min|max)\s*\(/i;
let pairs=0, same=0, rewriteOnly=0, other=0; const otherList=[];
for(const b of bases){
  const M=pick(names.filter(n=>n===b||new RegExp('^'+esc(b)+'_New\\d{6,8}$').test(n)));
  const W=pick(names.filter(n=>n===b+'_WH'||new RegExp('^'+esc(b)+'_WH_New\\d{6,8}$').test(n)));
  if(!M||!W) continue;
  if(macroCount(bodies.get(M))>0||macroCount(bodies.get(W))>0) continue;   // bo cap dinh macro
  pairs++;
  const a=sqlOf(bodies.get(M)), c=sqlOf(bodies.get(W));
  if(a.join('\n')===c.join('\n')){ same++; continue; }
  const sa=new Set(a), sc=new Set(c);
  const onlyM=a.filter(x=>!sc.has(x)), onlyW=c.filter(x=>!sa.has(x));
  const mAllTemp = onlyM.length>0 && onlyM.every(x=>tempRe.test(x));
  const wAllCorr = onlyW.length>0 && onlyW.every(x=>corrRe.test(x)||/^from\s/i.test(x));
  if(mAllTemp && wAllCorr){ rewriteOnly++; console.log('  VIET LAI: '+M+'  vs  '+W); }
  else { other++; otherList.push({M,W,om:onlyM.length,ow:onlyW.length}); }
}
console.log('cap macro-free so duoc='+pairs+'  giong het='+same
  +'  chi VIET LAI='+rewriteOnly+'  con lai CAN SOI='+other);
for(const x of otherList) console.log('  CAN SOI: '+x.M+' vs '+x.W+'  (main-only '+x.om+' / wh-only '+x.ow+')');
