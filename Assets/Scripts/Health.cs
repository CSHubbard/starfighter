using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
  [SerializeField] bool isPlayer;
  [SerializeField] int health = 50;
  [SerializeField] int score = 100;
  [SerializeField] ParticleSystem hitEffect;
  [SerializeField] bool applyCameraShake;
  CameraShake cameraShake;
  AudioPlayer audioPlayer;
  ScoreKeeper scoreKeeper;
  LevelManager levelManager;

  private void Awake()
  {
    cameraShake = Camera.main.GetComponent<CameraShake>();
    audioPlayer = FindObjectOfType<AudioPlayer>();
    scoreKeeper = FindObjectOfType<ScoreKeeper>();
    levelManager = FindObjectOfType<LevelManager>();
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    DamageDealer damageDealer = other.GetComponent<DamageDealer>();
    if (damageDealer != null)
    {
      TakeDamage(damageDealer.GetDamage());
      PlayHitEffect();
      audioPlayer.PlayDamageClip();
      ShakeCamera();
      damageDealer.Hit();
    }
  }

  private void ShakeCamera()
  {
    if (cameraShake != null && applyCameraShake)
    {
      cameraShake.Play();
    }
  }

  private void TakeDamage(int damage)
  {
    health -= damage;
    if (health <= 0)
    {
      Die();
    }
  }

  private void Die()
  {
    if (!isPlayer)
    {
      scoreKeeper.ModifyScore(score);
    }
    else
    {
      levelManager.LoadGameOver();
    }
    Destroy(gameObject);
  }

  private void PlayHitEffect()
  {
    if (hitEffect != null)
    {
      ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
      Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
    }
  }

  public int GetCurrentHealth()
  {
    return health;
  }
}
