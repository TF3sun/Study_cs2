using System;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Study_cs2
{
    /// <summary>
    /// 프로그램 진입점
    /// 
    /// [NuGet 패키지 필수]
    /// - Microsoft.Extensions.DependencyInjection (v8.0.0 이상)
    /// - Microsoft.Extensions.Hosting (v8.0.0 이상)
    /// 
    /// .NET 표준 DI 컨테이너를 사용하여 서비스를 등록하고,
    /// 자동 의존성 주입을 통해 느슨한 결합과 확장성을 확보합니다.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // 2. Main 메서드 최상단에 이 코드를 추가하여 콘솔 인코딩을 UTF-8로 고정합니다.
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8; // 입력 깨짐 방지용 (필요시)

            // ============================================================
            // [Step 1] DI 컨테이너(ServiceCollection) 생성
            // ============================================================
            var services = new ServiceCollection();

            // ============================================================
            // [Step 2] 서비스 등록 (DI 컨테이너에 서비스를 어떻게 생성할지 명시)
            // ============================================================

            // [Singleton 등록]
            // UserController는 Stateless 서비스입니다.
            // - 내부 상태를 보유하지 않음 (모든 데이터는 매개변수로 전달받음)
            // - 입출력 기능만 순수하게 제공
            // - 따라서 애플리케이션 전 생명 주기 동안 단 하나의 인스턴스만 필요
            // 
            // 이점:
            //   1) 메모리 효율성: 하나의 인스턴스를 재사용하여 메모리 절약
            //   2) 스레드 안전성: 상태가 없으므로 동시성 문제 없음
            //   3) 성능: 인스턴스 생성 오버헤드 제거
            services.AddSingleton<IUserController, UserController>();

            // [Transient 등록]
            // GameManager는 Stateful 서비스입니다.
            // - 내부 상태를 보유함 (targetNumber, attemptCount 등)
            // - 게임 라운드마다 새로운 난수와 시도 횟수를 관리
            // - 따라서 요청(게임 시작)할 때마다 새로운 인스턴스 필요
            // 
            // 이점:
            //   1) 상태 격리: 각 게임 라운드가 독립적인 상태 보유
            //   2) 버그 방지: 싱글톤 인스턴스의 전역 상태 오염 방지
            //   3) 테스트 용이성: 각 테스트마다 신선한 인스턴스 사용 가능
            services.AddTransient<GameManager>();

            // ============================================================
            // [Step 3] ServiceProvider 빌드 (DI 컨테이너 완성)
            // ============================================================
            var serviceProvider = services.BuildServiceProvider();

            // ============================================================
            // [Step 4] GameManager 인스턴스 해결(Resolve) 및 게임 실행
            // ============================================================
            // GetRequiredService: 등록된 서비스를 해결(의존성 자동 주입)
            // - IUserController를 요청하면, Singleton인 UserController 인스턴스 반환
            // - GameManager를 요청하면, 새로운 Transient 인스턴스 생성 후 반환
            //   (생성자에서 필요한 IUserController는 Singleton 인스턴스 주입)
            var gameManager = serviceProvider.GetRequiredService<GameManager>();

            // 게임 실행
            gameManager.Run();

            // ============================================================
            // [아키텍처 이점 요약]
            // ============================================================
            // 1. 의존성 역전 (DIP)
            //    - GameManager가 구체적인 UserController 클래스에 의존하지 않음
            //    - IUserController 인터페이스에만 의존 → 느슨한 결합
            // 
            // 2. 확장성 (Open/Closed Principle)
            //    - IUserController를 구현한 다른 클래스(예: WebUIController) 추가 가능
            //    - Program.cs의 등록 부분만 수정하면 됨 (기존 코드 수정 최소화)
            // 
            // 3. 테스트 용이성 (Testability)
            //    - Mock/Stub IUserController 구현으로 단위 테스트 가능
            //    - GameManager 로직을 입출력 없이 테스트 가능
            // 
            // 4. 동시성 안전성 (Concurrency Safety)
            //    - Singleton: Stateless이므로 여러 스레드에서 안전하게 공유
            //    - Transient: 각 요청/라운드마다 새 인스턴스이므로 상태 오염 없음
            // 
            // 5. 생명 주기 관리의 명확성
            //    - 각 서비스의 수명 주기를 코드로 명확히 표현
            //    - 새로운 개발자도 아키텍처 의도를 쉽게 이해
        }
    }
}
