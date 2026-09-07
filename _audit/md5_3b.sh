#!/usr/bin/env bash
# ===== #B69 CONG CU DO 3B AN TOAN =====
# Thay cho cac lenh grep/awk/md5sum go tay - chan 3 bay da gap trong phien B:
#   1. NEO GREP HONG => start rong => awk tinh md5 TU DONG 0 (md5 vo nghia ma van
#      "khop hai may"). Bay nay bat duoc o #B68 (khai bao thut le bang TAB, neo dung
#      8 dau cach). Luat C0-quadringentesimusquadragesimus.
#   2. GREP KHOP DONG COMMENT: `grep -n "public Foo("` van khop `//public Foo(`
#      => do md5 tren BAN CHET. Bat duoc o #B51. Luat C0-...decimusseptimus.
#   3. FILE RAC LAN TRONG CAY NGUON: Backup/, Delete.*, " - Copy", *sync-conflict*
#      (may 150 co 26 file sync-conflict). Luat C0-quadringentesimustricesimus.
#
# Dung:  ./md5_3b.sh <file> <ten-ham> [so-dong]
# Vi du: ./md5_3b.sh TERP.BizHTC/BizHTC.Report.cs RptSales_Delivery_01_New20190308 330
set -u

FILE="${1:-}"; NAME="${2:-}"; LINES="${3:-200}"
if [ -z "$FILE" ] || [ -z "$NAME" ]; then
    echo "Dung: $0 <file> <ten-ham> [so-dong]" >&2; exit 2
fi
if [ ! -f "$FILE" ]; then echo "LOI: khong thay file $FILE" >&2; exit 2; fi

# --- Bay 3: tu choi do tren file rac ---
case "$FILE" in
    *Backup/*|*/Delete.*|*" - Copy"*|*sync-conflict*)
        echo "🔴 TU CHOI: '$FILE' la FILE RAC (Backup/Delete/Copy/sync-conflict)." >&2
        echo "   Do md5 tren file nay KHONG chung minh duoc gi - tim file LIVE trong csproj." >&2
        exit 3;;
esac

# --- Tim moi dong khai bao, LOAI dong comment (bay 2) ---
HITS=$(grep -n "$NAME" "$FILE" | awk -F: -v n="$NAME" '{
    line=$0; sub(/^[0-9]+:/,"",line);
    t=line; sub(/^[[:space:]]+/,"",t);
    if (substr(t,1,2) != "//" && (t ~ /^(public|private|protected|internal|static)/)) print $1;
}')
NHIT=$(printf '%s\n' "$HITS" | grep -c . || true)

if [ "$NHIT" -eq 0 ]; then
    echo "🔴 START RONG: khong tim thay dong KHAI BAO (khong phai comment) cho '$NAME' trong $FILE" >&2
    echo "   => KHONG duoc tinh md5. Kiem lai ten ham (co the sai hau to / dao chu / ten #region)." >&2
    COMMENTED=$(grep -c "//.*$NAME" "$FILE" || true)
    [ "$COMMENTED" -gt 0 ] && echo "   Ghi chu: co $COMMENTED dong COMMENT chua ten nay - ham co the da chet." >&2
    exit 4
fi
if [ "$NHIT" -gt 1 ]; then
    echo "⚠️  CANH BAO: co $NHIT khai bao trung ten trong cung file (overload?):" >&2
    printf '%s\n' "$HITS" | while read -r l; do echo "     dong $l: $(sed -n "${l}p" "$FILE" | cut -c1-100)" >&2; done
    echo "   => Dang dung dong DAU TIEN. Neu form goi overload khac, truyen so dong thu cong." >&2
fi

START=$(printf '%s\n' "$HITS" | head -1)
END=$((START + LINES))
MD5=$(awk -v s="$START" -v e="$END" 'NR>=s && NR<e' "$FILE" | md5sum | cut -d' ' -f1)

echo "file=$FILE"
echo "name=$NAME"
echo "start=$START"
echo "lines=$LINES"
echo "decl=$(sed -n "${START}p" "$FILE" | cut -c1-110)"
echo "md5=$MD5"
