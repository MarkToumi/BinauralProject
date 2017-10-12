using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
	private PlayerController _pc;
	private GameController _gc;
	private int _shotPower;
	public int shotPower {
		set { _shotPower = value; }
	}
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
		_pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		_shotPower = 0;
		gameObject.SetActive(false);
	}

	IEnumerator BulletLost() {
		yield return new WaitForSecondsRealtime(2);
		this.gameObject.SetActive(false);
	}

	void OnCollisionEnter(Collision other)
	{
		Debug.Log(_shotPower);
		if(other.gameObject.tag == "Enemy") {
			var enemy = other.gameObject.GetComponent<Enemy>();
			enemy.life -= _shotPower;
			if(enemy.life <= 0) {
				_pc.score += enemy.crushingScore;
				_pc.ScoreUpdate();
				if(enemy.eType == EnemyType.Boss) {
					_gc.isPlay = false;
					_gc.state = PlayState.End;
				}
				other.gameObject.SetActive(false);
			}
			this.gameObject.SetActive(false);
		}
	}
}
