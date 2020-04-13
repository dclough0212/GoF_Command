using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace Command
{
    public class BankAccount
    {

        private int balance;
        private int overdrawLimit = -500;

        public BankAccount(int balance = 0)
        {
            this.balance = balance;
        }

        public bool Withdraw(int amount)
        {
            if (balance - amount >= overdrawLimit)
            {
                balance -= amount;
                WriteLine($"Withdrew ${amount} balance is now ${balance}");
                return true;
            }
            else
                return false;
        }

        public void Deposit(int amount)
        {
            balance += amount;
            WriteLine($"Deposited ${amount} balance is now ${balance}");
        }
    }


    public abstract class Command
    {
        public abstract void Call();
        public abstract void Undo();
        public bool Success;
    }

    public class BankAccountCommand : Command
    {
        private int amount;
        private Action action;
        private bool succeed;
        private BankAccount account;

        public BankAccountCommand(BankAccount account, int amount, Action action)
        {
            this.amount = amount;
            this.action = action;
            this.account = account;
        }

        public override void Call()
        {
            switch(this.action)
            {
                case Action.Deposit:
                    account.Deposit(amount);
                    succeed = true;
                    break;
                case Action.Withdraw:
                    succeed = account.Withdraw(amount);
                    break;
                default:
                    break;
            }
        }

        public override void Undo()
        {
            switch(this.action)
            {
                case Action.Deposit:
                    account.Withdraw(amount);
                    break;
                case Action.Withdraw:
                    account.Deposit(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum Action
        {
            Withdraw,
            Deposit
        }


    }


    class Program
    {
        static void Main(string[] args)
        {
            BankAccount ba = new BankAccount(0);
            BankAccountCommand command = new BankAccountCommand(ba, 100, BankAccountCommand.Action.Deposit);
            command.Call();
            BankAccountCommand cmd2 = new BankAccountCommand(ba, 1000, BankAccountCommand.Action.Withdraw);
            command.Call();
            cmd2.Call();

            ReadLine();
        }
    }
}
