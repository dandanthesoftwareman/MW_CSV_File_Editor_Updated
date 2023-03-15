using CSV_File_Editor_Updated;

Console.Write("Please enter a filepath: ");
string filePathInput = GetValidFilePath();
string filePathOutput = @$"{Path.GetDirectoryName(filePathInput)}\{Path.GetFileNameWithoutExtension(filePathInput)}_Formatted.csv";
List<Loan> loans = ReadCSVFile(filePathInput);
WriteCSVFile(filePathOutput, loans);

static string GetValidFilePath()
{
    string filePath = String.Empty;
    while (true)
    {
        filePath = Console.ReadLine().Replace("\"", "");
        if (File.Exists(filePath))
        {
            return filePath;
        }
        else
        {
            Console.Write("File not found, please enter a valid filepath: ");
        }
    }
}
static List<Loan> ReadCSVFile(string InputPath)
{
    StreamReader reader = new StreamReader(InputPath);
    List<Loan> loans = new List<Loan>();
    string expectedHeader = "AccountNumber,LoanId,LastName,FirstName,AmountDue,DateDue,SocialLastFour";
    while (true)
    {
        string line = reader.ReadLine();
        if (line == null)
        {
            break;
        }
        else if(line == expectedHeader)
        {
            continue;
        }
        else
        {
            string[] values = line.Split(',');
            if (values[4].Length == 0)
            {
                values[4] = values[4].Insert(0, "00");
            }
            if (values[4].Length == 1)
            {
                values[4] = values[4].Insert(0, "0");
            }
            Loan newLoan = new Loan(values[0], values[1], values[2], values[3], Convert.ToDecimal(values[4].Insert(values[4].Length -2, ".")), values[5], values[6]);
            loans.Add(newLoan);
        }
    }
    reader.Close();
    return loans;
}
static void WriteCSVFile(string OutputPath, List<Loan> loans)
{
    StreamWriter writer = new StreamWriter(OutputPath);

    writer.WriteLine("AccountNumber,LoanId,Name,AmountDue,DateDue,SocialLastFour");
    foreach (Loan loan in loans)
    {
        if (loan.AmountDue < 1000)
        {
            writer.WriteLine($"{loan.AccountNumber},{loan.LoanId},{loan.FirstName} {loan.LastName},{string.Format("{0:C}", loan.AmountDue)},{loan.DateDue},{loan.SocialLastFour}");
        }
        else
        {
            writer.WriteLine($"{loan.AccountNumber},{loan.LoanId},{loan.FirstName} {loan.LastName},\"{string.Format("{0:C}", loan.AmountDue)}\",{loan.DateDue},{loan.SocialLastFour}");
        }
    }
    writer.Close();
    Console.WriteLine();
    Console.WriteLine("File has been Formatted!");
}