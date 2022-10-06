using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

var regex = new Regex("data=\"(.*?)\"");
//("data=\"(.*?)\"");
string text = "Broadcasting: Intent { act=clipper.get flg=0x400000 }\r\nBroadcast completed: result=-1, data=\"Lâu lâu lại lên cây máy cỏ chất lượng cho ae.\r\nRealme 7Pro snap 720G ram 8.128gb màn Amoled vân tay trong màn pin max trâu lại còn loa kép bhanh chính hãng. Máy trần 3790k kèm sạc oppo 20w +150k.\r\nNhận kèo Vĩnh Tuy Hn 0936312486\"\r\n";
//if (regex.Match(text).Success)
//{
//    string token = regex.Match(text).Groups[2].Value.Trim('\0');
//    string filterToken = regexLetter.Replace(token, "");
string  result = string.Join(" ", Regex.Split(text, @"(?:\r\n|\n|\r|\\)"));
if (regex.Match(result).Success)
{
   string outttt = regex.Match(result).Groups[1].Value;
    Console.WriteLine(outttt);
}
else
{

Console.WriteLine("not match");
}