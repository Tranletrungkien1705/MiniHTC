// #468: So ban _WH voi ban MAIN **DANG SONG** (WS goi), khong phai voi ten tran.
// Ten tran co the la ban CHET => so nham se bao lech gia (su co #467).
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
// Tap ham duoc WebMethod goi (ban SONG)
const wsFiles=fs.readdirSync(WS).filter(x=>x.endsWith('.cs'));
console.log('  doc ' + wsFiles.length + ' file WS .cs');
const live=new Set();
for(const f of wsFiles){
  for(const L of fs.readFileSync(path.join(WS,f),'utf8').split(/\r?\n/)){
    if(/^\s*\/\//.test(L)) continue;
    const m=L.match(/_biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/); if(m) live.add(m[1]);
  }
}
console.log('  ham biz duoc WS goi = ' + live.size);

function sqlOf(lines){
  return lines.map(x=>x.replace(/\r/g,''))
    .filter(x=>!/^\s*\/\/\//.test(x))   // #469: bo dong XML-doc ///
    .filter(x=>/select|from\s|where|join|group by|order by|into\s/i.test(x))
    .map(x=>x.replace(/\[@strDBName_CommonCenter\]\.\[dbo\]\./g,'')
             .replace(/_WH\b/g,'').replace(/_New\d{6,8}/g,'')
             .replace(/with\s*\(\s*nolock\s*\)/ig,'--//[mylock]')
             .replace(/--(?!\/\/\[mylock\]).*$/,'')
             .replace(/\(\s*1\s*=\s*1\s*\)/g,'1=1')
             .replace(/\s+/g,' ').trim().toLowerCase())
    .filter(x=>x.length>0);
}

const whNames=[...bodies.keys()].filter(n=>/_WH$/.test(n));
let paired=0, same=0, noMain=0; const diff=[];
for(const w of whNames){
  const base=w.replace(/_WH$/,'');
  // ung vien: ten tran + moi bien the _New########
  const cands=[...bodies.keys()].filter(n=>n===base || n.startsWith(base+'_New'));
  if(!cands.length){ noMain++; continue; }
  // uu tien ban duoc WS goi; neu nhieu, lay ban nhieu dong SQL nhat
  let pick=cands.filter(n=>live.has(n));
  if(!pick.length) pick=cands;
  pick.sort((x,y)=>sqlOf(bodies.get(y)).length-sqlOf(bodies.get(x)).length);
  const main=pick[0];
  paired++;
  const a=sqlOf(bodies.get(main)).join('\n'), b=sqlOf(bodies.get(w)).join('\n');
  if(a===b) same++;
  else diff.push({w, main, la:sqlOf(bodies.get(main)).length, lb:sqlOf(bodies.get(w)).length,
                  liveMain:live.has(main)});
}
console.log('ham _WH=' + whNames.length + '  ghep duoc=' + paired + '  khong co main=' + noMain);
console.log('SQL GIONG HET=' + same + '  KHAC=' + diff.length);
const eq=diff.filter(d=>d.la===d.lb).length;
console.log('  trong so KHAC: cung so dong=' + eq + '  lech so dong=' + (diff.length-eq));
for(const d of diff.filter(x=>x.la!==x.lb))
  console.log('  LECH: ' + d.w + '  <- main=' + d.main + (d.liveMain?' (WS goi)':' (KHONG duoc WS goi)')
    + '  ' + d.la + ' -> ' + d.lb);
