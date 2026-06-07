using System;

namespace Study_cs2
{
    /// <summary>
    /// 사용자 인터페이스 제어 담당 (IUserController 구현)
    /// 모든 콘솔 입출력을 전담하며, GameManager와의 명확한 데이터 흐름을 유지합니다.
    /// (단일 책임 원칙: 입출력만 담당)
    /// 
    /// [DI 수명 주기: Singleton]
    /// 이 클래스는 상태(State)를 보유하지 않고 순수하게 입출력 기능만 제공하는
    /// Stateless 서비스이므로, 애플리케이션 전 생명 주기 동안 단 하나의 인스턴스만
    /// 생성되어 모든 GameManager 인스턴스에서 공유됩니다.
    /// 이를 통해 메모리 사용을 최소화하고 스레드 안전성을 보장합니다.
    /// </summary>
    public class UserController : IUserController
    {
        /// <summary>
        /// 게임 시작 메시지를 출력하고 준비 완료 신호를 반환합니다.
        /// </summary>
        /// <returns>true = 준비 완료</returns>
        public bool ShowStartMessage()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("       업/다운 숫자 맞추기 게임");
            Console.WriteLine("========================================");
            Console.WriteLine("1부터 100 사이의 숫자를 맞혀보세요!");
            Console.WriteLine("(최대 7회 시도 가능)");
            Console.WriteLine();

            // 준비 완료 신호를 GameManager에게 반환
            return true;
        }

        /// <summary>
        /// 사용자로부터 1~100 범위의 정수를 입력받습니다.
        /// 잘못된 입력(문자, 범위 초과)에 대해 자체적으로 재입력을 요청합니다.
        /// </summary>
        /// <returns>검증된 정수 (1~100)</returns>
        public int RequestNumberInput()
        {
            while (true)
            {
                Console.Write("숫자를 입력하세요(1~100): ");
                string inputStr = Console.ReadLine();

                // 1단계: 정수 파싱 시도
                if (!int.TryParse(inputStr, out int inputNum))
                {
                    Console.WriteLine("❌ 입력값이 정수가 아닙니다. 다시 입력해주세요.");
                    Console.WriteLine();
                    continue;
                }

                // 2단계: 범위 검증
                if (inputNum < 1 || inputNum > 100)
                {
                    Console.WriteLine($"❌ 입력값({inputNum})이 1~100 범위를 벗어났습니다. 다시 입력해주세요.");
                    Console.WriteLine();
                    continue;
                }

                // 검증 완료: 정상 입력값을 GameManager에게 반환
                Console.WriteLine($"✓ 입력값: {inputNum}");
                Console.WriteLine();
                return inputNum;
            }
        }

        /// <summary>
        /// GameManager의 판정 결과를 사용자에게 출력합니다.
        /// </summary>
        /// <param name="resultMessage">판정 결과 ("다운", "업", "정답입니다!")</param>
        /// <param name="attemptCount">현재까지의 시도 횟수</param>
        public void DisplayResult(string resultMessage, int attemptCount)
        {
            if (resultMessage == "정답입니다!")
            {
                Console.WriteLine("🎉 " + resultMessage);
            }
            else
            {
                Console.WriteLine("➤ " + resultMessage);
            }

            Console.WriteLine($"   (시도 횟수: {attemptCount}회)");
            Console.WriteLine();
        }

        /// <summary>
        /// 게임 종료 후 재시작 여부를 사용자에게 물어봅니다.
        /// </summary>
        /// <returns>true = 재시작, false = 종료</returns>
        public bool AskForRestart()
        {
            while (true)
            {
                Console.Write("다시 플레이하시겠습니까? (y/n): ");
                string response = Console.ReadLine()?.Trim().ToLower();

                if (response == "y" || response == "yes")
                {
                    Console.WriteLine();
                    return true;
                }
                else if (response == "n" || response == "no")
                {
                    Console.WriteLine();
                    return false;
                }
                else
                {
                    Console.WriteLine("❌ 잘못된 입력입니다. 'y' 또는 'n'을 입력해주세요.");
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// 게임 종료 메시지를 출력합니다.
        /// </summary>
        public void ShowExitMessage()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("         게임을 종료합니다.");
            Console.WriteLine("         플레이해주셔서 감사합니다!");
            Console.WriteLine("========================================");
        }

        /// <summary>
        /// 게임 실패 메시지를 출력합니다 (7회 초과 시).
        /// 최대 시도 횟수를 초과하여 게임에 실패했을 때 호출되며,
        /// 정답을 공개합니다.
        /// </summary>
        /// <param name="correctAnswer">정답 숫자</param>
        /// <param name="maxAttempts">최대 시도 횟수</param>
        public void DisplayFailureMessage(int correctAnswer, int maxAttempts)
        {
            Console.WriteLine("❌ {0}회 동안 정답을 맞히지 못해 실패했습니다! 정답은 [{1}]였습니다.",
                maxAttempts, correctAnswer);
            Console.WriteLine();
        }
    }
}
