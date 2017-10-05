using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	private Transform _bulletPos;
	private GameController _gc;
	private bool _isAlive = true;
	public bool isAlive {
		get { return _isAlive; }
	}

	private bool _isCoroutine = false;
	[SerializeField]
	private Text _scoreText;
	[SerializeField]
	private Slider _countSlider;

	private bool isFire {
		get { return Input.GetButtonDown("Fire1"); }
	}
	// Use this for initialization
	void Start () {
		_gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		_score = 0;
		_bulletCount = 0;
		_countSlider.maxValue = _maxCount;
		_countSlider.value = 0;
		BulletGenerate();
		ScoreUpdate();
	}
	
	// Update is called once per frame
	void Update () {
		if(_gc.isPlay) {
			if(isFire)
				Fire();
			GetDirection();
			if(_bulletCount >= _bullets.Count)
				StartCoroutine(BulletReload());
		}
	}

	void BulletGenerate() {
		_bulletPos = transform.FindChild("BulletPos");
		var bParent = GameObject.Find("Armoury");
		for(int i = 0; i < _maxCount; i++) {
			var bullet = Instantiate(_bullet, bParent.transform);
			bullet.transform.localPosition = new Vector3(0, 0, 5);
			_bullets.Add(bullet);
		}
	}

	void GetDirection() {
		var rotateSpeed = Input.GetAxis("Horizontal");
		var nowRotate = transform.rotation.eulerAngles;
		nowRotate.y += rotateSpeed;
		transform.rotation = Quaternion.Euler(nowRotate);
	}

	void Fire() {
		if(_bullets.Count > _bulletCount) {
			var bullet =_bullets[_bulletCount];
			bullet.SetActive(true);
			bullet.transform.position = _bulletPos.position;
			bullet.transform.localRotation = Quaternion.Euler(Vector3.zero);
			bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
			bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 25, ForceMode.Impulse);
			_bulletCount++;
			_countSlider.value = _bulletCount;
		}
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Enemy") {
			_isAlive = false;
			_gc.isPlay = false;
			other.gameObject.SetActive(false);
		}
	}

	IEnumerator BulletReload() {
		if(_isCoroutine)
			yield break;
		_isCoroutine = true;
		yield return new WaitForSecondsRealtime(2);
		_bulletCount = 0;
		_countSlider.value = 0;
		_isCoroutine = false;
		yield break;
	}

	public void ScoreUpdate() {
		_scoreText.text = _score.ToString();
	}
}
