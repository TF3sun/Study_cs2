using System;

namespace Study_cs2
{
    /// <summary>
    /// 사용자 인터페이스(I/O) 역할을 정의하는 계약(Contract).
    /// 이 인터페이스를 통해 GameManager는 구체적인 구현에 의존하지 않고,
    /// 추상화된 기능에만 의존합니다. (의존성 역전 원칙: DIP)
    /// </summary>
    public interface IUserController
    {
        /// <summary>
        /// 게임 시작 메시지를 출력하고 준비 완료 신호를 반환합니다.
        /// </summary>
        /// <returns>true = 준비 완료</returns>
        bool ShowStartMessage();

        /// <summary>
        /// 사용자로부터 1~100 범위의 정수를 입력받습니다.
        /// 잘못된 입력에 대해 자체적으로 재입력을 요청합니다.
        /// </summary>
        /// <returns>검증된 정수 (1~100)</returns>
        int RequestNumberInput();

        /// <summary>
        /// GameManager의 판정 결과를 사용자에게 출력합니다.
        /// </summary>
        /// <param name="resultMessage">판정 결과 ("다운", "업", "정답입니다!")</param>
        /// <param name="attemptCount">현재까지의 시도 횟수</param>
        void DisplayResult(string resultMessage, int attemptCount);

        /// <summary>
        /// 게임 종료 후 재시작 여부를 사용자에게 물어봅니다.
        /// </summary>
        /// <returns>true = 재시작, false = 종료</returns>
        bool AskForRestart();

        /// <summary>
        /// 게임 종료 메시지를 출력합니다.
        /// </summary>
        void ShowExitMessage();

        /// <summary>
        /// 게임 실패 메시지를 출력합니다 (7회 초과 시).
        /// </summary>
        /// <param name="correctAnswer">정답 숫자</param>
        /// <param name="maxAttempts">최대 시도 횟수</param>
        void DisplayFailureMessage(int correctAnswer, int maxAttempts);
    }
}
