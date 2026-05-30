using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study_cs2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int rand_num = new Random().Next(1, 100);
            int chance = 0;

            while (true)
            {
                int input_num = 0;
                Console.WriteLine("숫자를 입력하세요(1~100) : ");

                while (true)
                {
                    string input_str = Console.ReadLine();

                    if (int.TryParse(input_str, out input_num)){
                        Console.WriteLine("입력값 :" + input_num);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 값을 입력했습니다. 다시 입력해주세요");
                    }
                }

                if(input_num > rand_num)
                {
                    Console.WriteLine("다운");
                }
                else if (input_num < rand_num)
                {
                    Console.WriteLine("업");
                }
                else
                {
                    Console.WriteLine("정답입니다!");
                }
            }              
        }
    }
}
