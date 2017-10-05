using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyType {
	Easy = 0,
	Normal,
	Hard,
	Boss
};

public class Enemy : MonoBehaviour {
	private EnemyType _eType;
	public EnemyType eType {
		get { return _eType; }
		set { _eType = value; }
	}

	private int _crushingScore;
	public int crushingScore {
		get { return _crushingScore; }
	}

	private int _life;
	public int life {
		get { return _life; }
		set { _life = value; }
	}

	private float _speed;
	private GameObject _pObj;
	// Use this for initialization
	void Start () {
		_speed = 0.2f;
		_pObj = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.LookRotation(_pObj.transform.position - transform.position);
		transform.position += transform.forward * _speed;
	}
	public void EnemyInit() {
		switch(_eType) {
			case EnemyType.Easy:
				_crushingScore = 5;
				_life = 1;
				break;
			case EnemyType.Normal:
				_crushingScore = 10;
				_life = 2;
				break;
			case EnemyType.Hard:
				_crushingScore = 20;
				_life = 3;
				 break;
			case EnemyType.Boss:
				_crushingScore = 50;
				_life = 5;
				break;
			default:
				_crushingScore = 0;
				_life = 0;
				break;

		}
	}
}
