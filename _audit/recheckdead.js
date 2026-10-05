// #481: Ra lai MOI ket luan "CHET" da ghi trong Program.cs, doi chieu voi tap SONG day du (4 project WS).
const fs=require('fs'), path=require('path');
const PROG=process.argv[2], BIZ=process.argv[3], WS=process.argv[4];

function wsLiveNames(wsArg){
  const dirs=String(wsArg).split(',').map(x=>x.trim()).filter(Boolean);
  const out=new Map(); let files=0;
  const callRe=/_biz\.([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
  for(const d of dirs){
    const stack=[d];
    while(stack.length){
      const cur=stack.pop();
      for(const e of fs.readdirSync(cur,{withFileTypes:true})){
        const fp=path.join(cur,e.name);
        if(e.isDirectory()){ if(!/^(bin|obj)$/.test(e.name)) stack.push(fp); }
        else if(e.name.endsWith('.cs')){
          files++;
          for(const L of fs.readFileSync(fp,'utf8').split(/\r?\n/)){
            if(/^\s*\/\//.test(L)) continue;
            const m=L.match(callRe);
            if(m){ if(!out.has(m[1])) out.set(m[1], new Set()); out.get(m[1]).add(path.basename(d)); }
          }
        }
      }
    }
  }
  console.log('  doc ' + files + ' file WS tu ' + dirs.length + ' project; ten biz duoc goi = ' + out.size);
  return out;
}
const live = wsLiveNames(WS);

// Tap ten ham co that trong biz
function walk(d,a){for(const e of fs.readdirSync(d,{withFileTypes:true})){const fp=path.join(d,e.name);
 if(e.isDirectory()){if(!/^(bin|obj|Properties|Web References|Service References)$/.test(e.name))walk(fp,a);}
 else if(e.name.endsWith('.cs'))a.push(fp);}return a;}
const memberRe=/^\s*(?:public|private|protected|internal)\s+(?:static\s+)?[A-Za-z_][A-Za-z0-9_<>,\[\]\s]*\s+([A-Za-z_][A-Za-z0-9_]*)\s*\(/;
const bizNames=new Set();
const bizFiles=walk(BIZ,[]);
for(const f of bizFiles) for(const L of fs.readFileSync(f,'utf8').split(/\r?\n/)){
  if(/^\s*\/\//.test(L)) continue;
  const m=L.match(memberRe); if(m) bizNames.add(m[1]);
}
console.log('  doc ' + bizFiles.length + ' file biz; ten ham biz = ' + bizNames.size);

// Trich moi ten dat trong dau ` ` nam gan chu CHET/chet (trong 200 ky tu sau ten)
const prog=fs.readFileSync(PROG,'utf8');
const BT=String.fromCharCode(96);
const re=new RegExp(BT+'([A-Za-z_][A-Za-z0-9_]{5,})'+BT+'([^'+BT+']{0,120})','g');
const claimed=new Map();
let m;
while((m=re.exec(prog))!==null){
  if(!/CHẾT|chết|DEAD/.test(m[2])) continue;
  if(!bizNames.has(m[1])) continue;           // chi xet ten ham biz co that
  if(!claimed.has(m[1])) claimed.set(m[1], 0);
  claimed.set(m[1], claimed.get(m[1])+1);
}
console.log('ten ham biz bi ghi la CHET trong Program.cs = ' + claimed.size);
let wrong=0, right=0;
for(const [n] of [...claimed].sort()){
  if(live.has(n)){ wrong++; console.log('  🔴 SAI: ' + n + '  -> THUC RA SONG, goi tu: ' + [...live.get(n)].join(', ')); }
  else right++;
}
console.log('==> dung ' + right + ' / sai ' + wrong);
