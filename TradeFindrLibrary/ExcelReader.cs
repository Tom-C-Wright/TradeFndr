using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TradeFindr
{
    public class ExcelReader
    {
        public ExcelReader()
        {

        }

        public Trade[] ReadXlsx(IExcelDataReader reader)
        {
            List<Trade> result = new List<Trade>();
            do
            {
                while (reader.Read())
                {
                    try
                    {
                      
                        var time = reader.GetValue(0);
                        var price = reader.GetValue(1);
                        var volume = reader.GetValue(2);
                        var value = reader.GetValue(3);
                        var reasonString = reader.GetValue(4);

                        if (time != null && price != null && volume != null && value != null && reasonString != null)
                        {
                            Reason reason;
                            switch (reasonString)
                            {
                                case "ASK":
                                    reason = Reason.ASK;
                                    break;
                                case "BID":
                                    reason = Reason.BID;
                                    break;
                                case "MATCH":
                                    reason = Reason.MATCH;
                                    break;
                                default:
                                    reason = Reason.ASK;
                                    break;
                            }

                            result.Add(new Trade(time: Convert.ToDateTime(time), price: Convert.ToDouble(price), volume: Convert.ToDouble(volume), value: Convert.ToDouble(value), reason: reason));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        continue; // Try to read the next row
                    }
                }
            } while (reader.NextResult());
            return result.ToArray();
        }

        public Trade[] ReadCSV(IExcelDataReader reader)
        {
            List<Trade> result = new List<Trade>();
            do
            {
                while (reader.Read() && reader != null)
                {
                    try
                    {
                        var x = reader;
                        DateTime time = DateTime.Parse(reader.GetString(0));
                        double price = Double.Parse(reader.GetString(1));
                        double volume = Double.Parse(reader.GetString(2));
                        double value = Double.Parse(reader.GetString(3));
                        string reasonString = reader.GetString(4);
                        Reason reason;
                        switch (reasonString)
                        {
                            case "ASK":
                                reason = Reason.ASK;
                                break;
                            case "BID":
                                reason = Reason.BID;
                                break;
                            case "MATCH":
                                reason = Reason.MATCH;
                                break;
                            default:
                                reason = Reason.ASK;
                                break;
                        }
                        if (time != null || price != 0 || volume != 0 || value != 0)
                        {
                            result.Add(new Trade(time: time, price: price, value: value, volume: volume, reason: reason));
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        continue; // Try to read the next row
                    }

                }
            } while (reader.NextResult());
            return result.ToArray();
        }
        public Trade[] ReadFile(string path)
        {
            try
            {
                // Encoding Support for Windows-1252
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {

                    string type = Path.GetExtension(path);
                    if (type == ".csv")
                    {
                        using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                        {
                            return ReadCSV(reader);
                        }
                    }
                    else if (type == ".xlsx")
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            return ReadXlsx(reader);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Filetype not .csv or .xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw new IOException("File cannot be read because it is open in another program");
            }
        }

    }
}
