// #471: So ban MAIN dang song voi ban _WH DANG SONG.
// CA HAI phia deu co the mang hau to ngay (X_New######## va X_WH_New########);
// ten tran thuong la ban CHET => phai chon theo tap ham duoc WebMethod goi.
const fs=require('fs'),path=require('path');
// #478: tap ham SONG phai gop CA BON project WS, khong chi HTCWSCarSv.
function wsLiveNames(wsArg){
  const dirs = String(wsArg).split(",").map(x=>x.trim()).filter(Boolean);
  const out = new Set(); let files = 0;
  const callRe = /_biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
  const cmtRe = /^\s*\/\//;
  for (const d of dirs) {
    const stack=[d];
    while(stack.length){
      const cur=stack.pop();
      for(const e of fs.readdirSync(cur,{withFileTypes:true})){
        const fp=path.join(cur,e.name);
        if(e.isDirectory()){ if(!/^(bin|obj)$/.test(e.name)) stack.push(fp); }
        else if(e.name.endsWith(".cs")){
          files++;
          for(const L of fs.readFileSync(fp,"utf8").split(/\r?\n/)){
            if(cmtRe.test(L)) continue;
            const m=L.match(callRe); if(m) out.add(m[1]);
          }
        }
      }
    }
  }
  console.log("  doc "+files+" file WS tu "+dirs.length+" project; ten biz duoc goi = "+out.size);
  return out;
}
const WSDIRS = true;

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
const live = wsLiveNames(WS);

// #475: SQL sinh boi macro zzB_..._zzE KHONG dem duoc bang dong chu.
// Ham nay dem so cho dung macro de moi phep so deu co canh bao.
function macroCount(lines){ return lines.filter(x=>/zzB_[A-Za-z0-9_]+_zzE/.test(x)).length; }

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
function pick(cands){
  if(!cands.length) return null;
  let p=cands.filter(n=>live.has(n));
  if(!p.length) return null;                       // khong ban nao SONG => bo qua, khong doan
  p.sort((x,y)=>sqlOf(bodies.get(y)).length-sqlOf(bodies.get(x)).length);
  return p[0];
}
// Tap base: moi ten co bien the _WH
const bases=new Set();
for(const n of names){
  const m=n.match(/^(.*)_WH(_New\d{6,8})?$/); if(m) bases.add(m[1]);
}
let considered=0, skipped=0, same=0; const diff=[]; let macroPairs=0;
for(const b of bases){
  const mainC=names.filter(n=>n===b || new RegExp('^'+b.replace(/[.*+?^${}()|[\]\\]/g,'\\$&')+'_New\\d{6,8}$').test(n));
  const whC  =names.filter(n=>n===b+'_WH' || new RegExp('^'+b.replace(/[.*+?^${}()|[\]\\]/g,'\\$&')+'_WH_New\\d{6,8}$').test(n));
  const M=pick(mainC), W=pick(whC);
  if(!M||!W){ skipped++; continue; }
  considered++;
  if(macroCount(bodies.get(M))>0||macroCount(bodies.get(W))>0) macroPairs++;
  const a=sqlOf(bodies.get(M)).join('\n'), c=sqlOf(bodies.get(W)).join('\n');
  if(a===c) same++;
  else diff.push({b,M,W,la:sqlOf(bodies.get(M)).length,lb:sqlOf(bodies.get(W)).length});
}
console.log('base co bien the _WH=' + bases.size + '  so duoc (CA HAI phia deu SONG)=' + considered
  + '  bo qua (mot phia khong co ban song)=' + skipped);
console.log('SQL GIONG HET=' + same + '  KHAC=' + diff.length);
console.log('CANH BAO: cap co dung macro zzB_..._zzE (so dong SQL KHONG day du) = ' + macroPairs);
const eq=diff.filter(d=>d.la===d.lb).length;
console.log('  trong KHAC: cung so dong=' + eq + '  lech so dong=' + (diff.length-eq));
for(const d of diff.filter(x=>x.la!==x.lb).sort((x,y)=>Math.abs(y.la-y.lb)-Math.abs(x.la-x.lb)))
  console.log('  LECH: ' + d.M + '  vs  ' + d.W + '   ' + d.la + ' -> ' + d.lb);
