public enum EPlayerState
{
    Idle, //기본 상태 (완전 정지)
    Run,  //달리는 시작하는 상태
    RunStop, //달리는 걸 멈추고 살짝 제동 걸리게 되는 상태
    Air, //점프로 인한 체공 또는 떨어짐으로서 생기는 체공 상태
    Dead, // 죽음
    Jump
}

public enum EPlayerInputState{
    Run,
    Jump,
    Launch,
    RopeMove
}

public enum EPlayerCurrentState
{
    Idle,
    Air,
    Dash,
    Rope,
    Dead
}
