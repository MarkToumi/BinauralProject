using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
	private PlayerController _pc;
	private GameController _gc;
	// Use this for initialization
	void OnEnable()
	{
		if(_pc == null)
			return;
		else
			StartCoroutine(BulletLost());
	}
	void Start () {
		_gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		_pc = transform.parent.gameObject.GetComponent<PlayerController>();
		gameObject.SetActive(false);
	}

	IEnumerator BulletLost() {
		yield return new WaitForSecondsRealtime(2);
		_pc.bulletCount--;
		if(_pc.bulletCount < 0)
			_pc.bulletCount = 0;
		this.gameObject.SetActive(false);
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Enemy") {
			var enemy = other.gameObject.GetComponent<Enemy>();
			if(enemy.life <= 0) {
				_pc.score += enemy.crushingScore;
				if(enemy.eType == EnemyType.Boss) {
					_gc.isPlay = false;
				}
				other.gameObject.SetActive(false);
			}
			else {
				enemy.life--;
			}
			_pc.bulletCount--;
			if(_pc.bulletCount < 0)
			_pc.bulletCount = 0;
			this.gameObject.SetActive(false);
		}
	}
}
