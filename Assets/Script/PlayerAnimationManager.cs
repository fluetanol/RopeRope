using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator _playerAnimator;
    private SpriteRenderer _playerSprite;

    // Update is called once per frame
    void Start(){
        _playerAnimator = GetComponent<Animator>();
        _playerSprite = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        float playSpeed = Math.Abs(PlayerStatus.CurrentMoveDirection.x * PlayerStatus.Speed * 1/8);
        _playerAnimator.SetFloat("speed", playSpeed);

        if(PlayerStateManager.Instance.PlayerInputState == EPlayerInputState.Jump)  _playerAnimator.SetBool("isJump", true);
        else _playerAnimator.SetBool("isJump", false);


        if (PlayerStatus.CurrentMoveDirection.x < 0) _playerSprite.flipX = true;
        else if (PlayerStatus.CurrentMoveDirection.x > 0) _playerSprite.flipX = false;
    }
}
