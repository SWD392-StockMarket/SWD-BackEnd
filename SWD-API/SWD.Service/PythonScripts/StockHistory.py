import json
import pandas as pd
from vnstock import Vnstock
import logging
import sys

# Vô hiệu hóa logging
logging.disable(logging.CRITICAL)

def get_stock_data(symbol, start_date, end_date):
    try:
        stock = Vnstock().stock(symbol=symbol, source="TCBS")
        df = stock.quote.history(start=start_date, end=end_date, interval="1D")

        if df.empty:
            print(json.dumps([]))  # Trả về mảng rỗng thay vì không có gì
        else:
            print(df.to_json(orient="records"))

        sys.stdout.flush()  # Đảm bảo output chỉ có JSON
    except Exception as e:
        print(json.dumps({"error": str(e)}))
        sys.stdout.flush()

if __name__ == "__main__":
    if len(sys.argv) < 4:
        print(json.dumps({"error": "Missing arguments"}))
    else:
        symbol = sys.argv[1]
        start_date = sys.argv[2]
        end_date = sys.argv[3]
        get_stock_data(symbol, start_date, end_date)
