// #475: Bao nhieu cap _WH co khoi "ton dau ky" (#tbl_sd/#tbl_sdo/#tbl_Open*) ma ban chinh KHONG co?
const fs=require('fs'),path=require('path');
const BIZ=process.argv[2], WS=process.argv[3];
function walk(d,a){for(const e of fs.readdirSync(d,{withFileTypes:true})){const fp=path.join(d,e.name);
 if(e.isDirectory()){if(!/^(bin|obj|Properties|Web References|Service References)$/.test(e.name))walk(fp,a);}
 else if(e.name.endsWith('.cs'))a.push(fp);}return a;}
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const files=walk(BIZ,[]);
console.log('  doc ' + files.length + ' file biz .cs');
const bodies=new Map();
for(const f of files){
  const lines=fs.readFileSync(f,'utf8').split(/\r?\n/); const mem=[];
  lines.forEach((L,i)=>{const m=L.match(memberRe); if(m&&!/^\s*\/\//.test(L)) mem.push({i,name:m[1]});});
  for(let k=0;k<mem.length;k++){
    const from=mem[k].i, to=(k+1<mem.length?mem[k+1].i:lines.length);
    if(!bodies.has(mem[k].name)) bodies.set(mem[k].name, lines.slice(from,to));
  }
}
const wsFiles=fs.readdirSync(WS).filter(x=>x.endsWith('.cs'));
const live=new Set();
for(const f of wsFiles) for(const L of fs.readFileSync(path.join(WS,f),'utf8').split(/\r?\n/)){
  if(/^\s*\/\//.test(L)) continue;
  const m=L.match(/_biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/); if(m) live.add(m[1]);
}
console.log('  ham biz duoc WS goi = ' + live.size);
const names=[...bodies.keys()];
function pick(c){ const p=c.filter(n=>live.has(n)); if(!p.length) return null;
  p.sort((x,y)=>bodies.get(y).length-bodies.get(x).length); return p[0]; }
const esc=x=>x.replace(/[.*+?^${}()|[\]\\]/g,'\\$&');
// Dau hieu "ton dau ky": co dong khai bao bang tam mo dau ky
const openRe=/into\s+#tbl_(sd|sdo|open)/i;
function hasOpen(n){ return bodies.get(n).some(L=>!/^\s*\/\//.test(L) && openRe.test(L)); }

const bases=new Set();
for(const n of names){ const m=n.match(/^(.*)_WH(_New\d{6,8})?$/); if(m) bases.add(m[1]); }
let pairs=0, bothHave=0, onlyWh=0, onlyMain=0, neither=0; const list=[];
for(const b of bases){
  const M=pick(names.filter(n=>n===b || new RegExp('^'+esc(b)+'_New\\d{6,8}$').test(n)));
  const W=pick(names.filter(n=>n===b+'_WH' || new RegExp('^'+esc(b)+'_WH_New\\d{6,8}$').test(n)));
  if(!M||!W) continue;
  pairs++;
  const hm=hasOpen(M), hw=hasOpen(W);
  if(hm&&hw) bothHave++;
  else if(!hm&&hw){ onlyWh++; list.push({M,W}); }
  else if(hm&&!hw) onlyMain++;
  else neither++;
}
console.log('cap so duoc=' + pairs + '  ca hai co=' + bothHave + '  CHI _WH co=' + onlyWh
  + '  chi MAIN co=' + onlyMain + '  ca hai khong=' + neither);
for(const x of list) console.log('  CHI _WH: ' + x.W + '   (main=' + x.M + ')');
