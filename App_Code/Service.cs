
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    SqlConnection con = new SqlConnection(@"server=LAPTOP-1NL8389O\SQLEXPRESS;database=Ecommerce App;integrated security=true");



    // WCF Service: PaymentService





    // Method to check the balance in the Account_Tab
    public decimal CheckBalance(int usrId)
    {
        decimal balance = 0;
        string query = "SELECT Bal_Amount FROM Account_table WHERE Usr_Id = @usrId";

        if (con.State == ConnectionState.Closed)
            con.Open();

        SqlCommand cmd = new SqlCommand(query, con);
        cmd.Parameters.AddWithValue("@usrId", usrId);

        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            balance = Convert.ToDecimal(reader["Bal_Amount"]);
        }
        reader.Close(); // Close the reader after use

        return balance;
    }

    // Method to update the balance after a successful payment
    public string UpdateBalance(int usrId, decimal amountToPay)
    {
        string result = "";
        decimal currentBalance = CheckBalance(usrId); // Get current balance

        if (currentBalance >= amountToPay)
        {
            decimal newBalance = currentBalance - amountToPay;
            string updateQuery = "UPDATE Account_table SET Bal_Amount = @newBalance WHERE Usr_Id = @usrId";

            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand cmd = new SqlCommand(updateQuery, con);
            cmd.Parameters.AddWithValue("@newBalance", newBalance);
            cmd.Parameters.AddWithValue("@usrId", usrId);

            int rowsAffected = cmd.ExecuteNonQuery();

            result = rowsAffected > 0 ? "Payment successful, balance updated." : "Error updating balance.";
        }
        else
        {
            result = "Insufficient balance";
        }

        return result;
    }

    // Method to process the payment (checks balance and updates if sufficient)
    public string ProcessPayment(int usrId, decimal amountToPay)
    {
        string balanceStatus = UpdateBalance(usrId, amountToPay); // Update the balance
        return balanceStatus;
    }




    public string GetData(int value)
	{
		return string.Format("You entered: {0}", value);
	}

	public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}
}
