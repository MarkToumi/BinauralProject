using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	[SerializeField]
	private int _maxCount;
	[SerializeField]
	private GameObject _bullet;
	private List<GameObject> _bullets = new List<GameObject>();
	private int _bulletCount;
	public int bulletCount {
		get { return _bulletCount; }
		set { _bulletCount = value; }
	}
	private int _score;
	public int score {
		get { return _score; }
		set { _score = value; }
	}
	private GameController _gc;
	private bool _isAlive = true;
	public bool isAlive {
		get { return _isAlive; }
	}

	private bool isFire {
		get { return Input.GetButtonDown("Fire1"); }
	}
	// Use this for initialization
	void Start () {
		_gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		_score = 0;
		_bulletCount = 0;
		BulletGenerate();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isFire){
			Fire();
		}
	}

	void BulletGenerate() {
		for(int i = 0; i < _maxCount; i++) {
			var bullet = Instantiate(_bullet, this.transform);
			bullet.transform.localPosition = new Vector3(0, 0, 5);
			_bullets.Add(bullet);
		}
	}

	void Fire() {
		if(_bullets.Count > _bulletCount) {
			_bullets[_bulletCount].SetActive(true);
			_bullets[_bulletCount].transform.position = new Vector3(0, 0, 2);
			_bullets[_bulletCount].transform.localRotation = Quaternion.Euler(Vector3.zero);
			_bullets[_bulletCount].GetComponent<Rigidbody>().AddForce(Vector3.forward * 10, ForceMode.Impulse);
			_bulletCount++;
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Enemy") {
			_isAlive = false;
			_gc.isPlay = false;
		}
	}
}
