using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationManager : MonoBehaviour
{
    public CombatStateManager combatStateManager;

    //animator
    public Animator lucienAnimator;
    public Animator enemyAnimator;

    //animator options for each enemy
    public Animator thiefAnimator;

    public void setEnemyAnimator(int number)
    {
        switch(number)
        {
            case 1:
                enemyAnimator = thiefAnimator;
                break;
        }
    }

    //--------------------------------------------------CHARACTERS BOUNCING-----------------------------------------------------
    //character bouncing animation
    public void ApplyBounce()
    {
        // Start squish and pop animations for player and enemy
        StartCoroutine(BounceCoroutine(combatStateManager.player));
        StartCoroutine(BounceCoroutine(combatStateManager.enemy));
    }

    // Coroutine to animate the squish and pop effect
    private IEnumerator BounceCoroutine(GameObject avatar)
    {
        // Store the original scale and position of the avatar
        Vector3 originalScale = avatar.transform.localScale;
        Vector3 originalPosition = avatar.transform.position;

        float squishAmount = 0.97f;  // How much to squish (less than 1)
        float bounceDuration = 0.1f; // How long the squish/pop lasts

        // Adjust the scale to squish the top of the avatar
        avatar.transform.localScale = new Vector3(originalScale.x, originalScale.y * squishAmount, originalScale.z);

        // Move the avatar down to keep the bottom anchored
        avatar.transform.position = new Vector3(originalPosition.x, originalPosition.y - (originalScale.y - originalScale.y * squishAmount) / 2, originalPosition.z);

        // Wait for a short time to maintain the squish
        yield return new WaitForSeconds(bounceDuration / 2);

        // Restore the scale and position back to the original
        avatar.transform.localScale = originalScale;
        avatar.transform.position = originalPosition;

        // Wait until the bounce duration is complete
        yield return new WaitForSeconds(bounceDuration / 2);
    }

    //--------------------------------------------------LUCIEN-------------------------------------------------------------
    public void LucienPlayAttackAnimation()
    {
        lucienAnimator.SetBool("isAttacking", true);
        StartCoroutine(LucienResetAttackAnimation());
    }

    private IEnumerator LucienResetAttackAnimation()
    {
        // Wait until the attack animation is playing and it's completed
        yield return new WaitForSeconds(lucienAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Set isAttack to false after the animation is done
        lucienAnimator.SetBool("isAttacking", false);
    }

    public void LucienPlayDefendAnimation()
    {
        lucienAnimator.SetBool("isDefending", true);
        StartCoroutine(LucienResetDefendAnimation());
    }

    private IEnumerator LucienResetDefendAnimation()
    {
        // Wait until the attack animation is playing and it's completed
        yield return new WaitForSeconds(lucienAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Set isAttack to false after the animation is done
        lucienAnimator.SetBool("isDefending", false);
    }

    public void LucienPlayHurtAnimation()
    {
        lucienAnimator.SetBool("isHurt", true);

        // Get the player's SpriteRenderer and store the original color
        SpriteRenderer spriteRenderer = combatStateManager.player.GetComponent<SpriteRenderer>();

        // Change color to red
        spriteRenderer.color = Color.red;

        StartCoroutine(LucienResetHurtAnimation(spriteRenderer));
    }

    private IEnumerator LucienResetHurtAnimation(SpriteRenderer spriteRenderer)
    {
        // Wait until the hurt animation is completed
        yield return new WaitForSeconds(lucienAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Restore original color and reset animation flag
        spriteRenderer.color = Color.white;
        lucienAnimator.SetBool("isHurt", false);
    }
}
