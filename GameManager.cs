using System;

namespace Study_cs2
{
    /// <summary>
    /// 게임 규칙 및 상태 관리 담당
    /// IUserController 인터페이스(추상화)에 의존하여 느슨한 결합을 구현합니다.
    /// (단일 책임 원칙: 게임 흐름 및 판정 로직만 담당)
    /// 
    /// [DI 수명 주기: Transient]
    /// 이 클래스는 상태를 보유합니다 (targetNumber, attemptCount 등).
    /// 게임이 시작될 때마다 새로운 난수와 시도 횟수를 초기화해야 하므로,
    /// 요청 시마다 새로운 인스턴스를 생성하는 Transient로 등록합니다.
    /// 이를 통해 싱글톤 인스턴스에서 발생할 수 있는 전역 상태 오염(Global State Pollution)을
    /// 방지하고, 각 게임 라운드가 독립적인 상태를 유지하도록 보장합니다.
    /// </summary>
    public class GameManager
    {
        private readonly IUserController _userController;
        private readonly Random _random;

        /// <summary>
        /// GameManager 생성자 (기본: Random 자동 생성)
        /// </summary>
        /// <param name="userController">IUserController 인터페이스 구현 객체
        /// (DI 컨테이너가 자동으로 주입함)</param>
        public GameManager(IUserController userController)
        {
            _userController = userController ?? throw new ArgumentNullException(nameof(userController));
            _random = new Random();
        }

        /// <summary>
        /// GameManager 생성자 (테스트용: Random 주입 가능)
        /// 이 오버로드는 단위 테스트에서 난수를 제어하기 위해 사용됩니다.
        /// </summary>
        /// <param name="userController">IUserController 인터페이스 구현 객체</param>
        /// <param name="random">테스트용 Random 객체 또는 제어된 난수 생성기</param>
        public GameManager(IUserController userController, Random random)
        {
            _userController = userController ?? throw new ArgumentNullException(nameof(userController));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        /// <summary>
        /// 게임을 실행합니다.
        /// 전체 흐름: 시작 메시지 → 난수 생성 → 입력 → 판정 → 반복 또는 종료
        /// 
        /// [수정사항]
        /// - 최대 시도 횟수를 7회로 제한
        /// - 7회를 초과하면 게임 실패로 처리하고 정답 공개
        /// </summary>
        public void Run()
        {
            // [Flow 2-3단계] UC에게 게임 시작 메시지 출력 요청 및 준비 완료 신호 확인
            bool isReady = _userController.ShowStartMessage();

            if (!isReady)
            {
                // 준비가 안 되면 게임 종료
                _userController.ShowExitMessage();
                return;
            }

            // 외부 루프: 게임 재시작을 위한 루프
            while (true)
            {
                // [Flow 2단계] 난수 생성 (1~100)
                int targetNumber = _random.Next(1, 101);
                int attemptCount = 0;
                bool isAnswerCorrect = false;

                // ========== [추가 기능] 최대 시도 횟수 제한 (7회) ==========
                const int MAX_ATTEMPTS = 7;

                // 내부 루프: 정답을 맞히거나 7회 초과할 때까지 반복
                while (!isAnswerCorrect && attemptCount < MAX_ATTEMPTS)
                {
                    // [Flow 4-5단계] UC에게 숫자 입력 요청 → 검증된 정수 값 반환받음
                    int userGuess = _userController.RequestNumberInput();
                    attemptCount++;

                    // [Flow 6단계] 사용자 입력값과 난수 비교하여 판정
                    string resultMessage = JudgeGuess(userGuess, targetNumber);

                    // 정답 여부 판단
                    if (resultMessage == "정답입니다!")
                    {
                        isAnswerCorrect = true;
                    }

                    // [Flow 6-7단계] 판정 결과를 UC에게 전달하여 출력
                    _userController.DisplayResult(resultMessage, attemptCount);
                }

                // ========== [추가 기능] 7회 초과로 인한 게임 실패 처리 ==========
                // 루프가 종료되었는데 정답을 맞히지 못한 경우 (attemptCount >= MAX_ATTEMPTS && !isAnswerCorrect)
                if (!isAnswerCorrect)
                {
                    // 실패 메시지 출력 (정답 공개)
                    _userController.DisplayFailureMessage(targetNumber, MAX_ATTEMPTS);
                }

                // [Flow 8단계] 정답/실패 후 재시작 여부 확인
                bool wantRestart = _userController.AskForRestart();

                if (!wantRestart)
                {
                    // 게임 종료
                    _userController.ShowExitMessage();
                    break; // 외부 루프 탈출
                }

                // 재시작: 다시 새로운 게임 시작
            }
        }

        /// <summary>
        /// 사용자의 입력값과 난수를 비교하여 판정 결과를 반환합니다.
        /// </summary>
        /// <param name="userGuess">사용자가 입력한 숫자</param>
        /// <param name="targetNumber">정답 숫자 (난수)</param>
        /// <returns>판정 결과 문자열</returns>
        private string JudgeGuess(int userGuess, int targetNumber)
        {
            if (userGuess > targetNumber)
            {
                // 입력값이 난수보다 크면 더 작은 숫자를 입력해야 함
                return "다운";
            }
            else if (userGuess < targetNumber)
            {
                // 입력값이 난수보다 작으면 더 큰 숫자를 입력해야 함
                return "업";
            }
            else
            {
                // 입력값이 난수와 같으면 정답
                return "정답입니다!";
            }
        }
    }
}
