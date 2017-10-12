using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum WeaponType {
	Single = 0,
	Burst
}

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

	private bool _isReload = false;
	private bool _isShot = false;
	[SerializeField]
	private Text _scoreText;
	[SerializeField]
	private Text _weaponText;
	[SerializeField]
	private Slider _countSlider;
	private WeaponType _weapon;
	private int _wNum;
	private bool _isWeapon = false;
	// Use this for initialization
	void Start() {
		_gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		_score = 0;
		_bulletCount = 0;
		_countSlider.maxValue = _maxCount;
		_countSlider.value = 0;
		_weapon = WeaponType.Single;
		_wNum = (int)_weapon;
		_weaponText.text = _weapon.ToString ();
		BulletGenerate ();
		ScoreUpdate ();
	}
	
	// Update is called once per frame
	void Update() {
		if (_gc.isPlay && _gc.state == PlayState.Play) {
			if (Input.GetButtonDown ("Fire1"))
				Fire ();
			GetDirection ();
			WeaponChange ();
			if (_bulletCount >= _bullets.Count)
				StartCoroutine (BulletReload ());
		}
	}

	void BulletGenerate() {
		_bulletPos = transform.FindChild ("BulletPos");
		var bParent = GameObject.Find ("Armoury");
		for (int i = 0; i < _maxCount; i++) {
			var bullet = Instantiate (_bullet, bParent.transform);
			bullet.transform.localPosition = new Vector3(0, 0, 5);
			_bullets.Add (bullet);
		}
	}

	void GetDirection() {
		var rotateSpeed = Input.GetAxis ("Horizontal");
		var nowRotate = transform.rotation.eulerAngles;
		nowRotate.y += rotateSpeed;
		transform.rotation = Quaternion.Euler (nowRotate);
	}

	void WeaponChange() {
		var inpV = Input.GetAxisRaw ("Vertical");
		if (!_isWeapon) {
			if (Mathf.Abs (inpV) > 0.2f) {
				_isWeapon = true;
				if (inpV > 0.5f)
					_wNum--;
				else
					_wNum++;
				if (_wNum >= _weapon.Size ())
					_wNum = 0;
				else if (_wNum < -0.5f)
					_wNum = _weapon.Size () - 1;
			}
		}
		_weapon = (WeaponType)_wNum;
		_weaponText.text = _weapon.ToString ();
		if (inpV < 0.1f && inpV > -0.1f)
			_isWeapon = false;
	}

	void Fire() {
		if (_bullets.Count > _bulletCount) {
			if (_weapon == WeaponType.Single)
				StartCoroutine (SingleMode ());
			else if (_weapon == WeaponType.Burst)
				StartCoroutine (BurstMode ());
		}
	}

	IEnumerator SingleMode() {
		if (_isShot)
			yield break;
		_isShot = true;
		Shoot (_bulletCount);
		_bulletCount++;
		_countSlider.value = _bulletCount;
		yield return new WaitForSecondsRealtime(1f);
		_isShot = false;
		yield break;
	}

	IEnumerator BurstMode() {
		if (_isShot)
			yield break;
		_isShot = true;
		int shotNum = _bulletCount + 3 >= _maxCount ? _maxCount : _bulletCount + 3;
		for (int i = _bulletCount; i < shotNum; i++) {
			Shoot (i);
			yield return new WaitForSecondsRealtime(0.2f);
		}
		_bulletCount += 3;
		_countSlider.value = _bulletCount;
		_isShot = false;
		yield break;
	}

	void Shoot(int bulletNum) {
		var bullet = _bullets [bulletNum];
		bullet.SetActive (true);
		bullet.GetComponent<PlayerBullet> ().shotPower = BulletPower (_weapon);
		bullet.transform.position = _bulletPos.position;
		bullet.transform.localRotation = Quaternion.Euler (Vector3.zero);
		bullet.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		bullet.GetComponent<Rigidbody> ().AddForce (transform.forward * 25, ForceMode.Impulse);
	}

	IEnumerator BulletReload() {
		if (_isReload)
			yield break;
		_isReload = true;
		yield return new WaitForSecondsRealtime(2);
		_bulletCount = 0;
		_countSlider.value = 0;
		_isReload = false;
		yield break;
	}

	int BulletPower(WeaponType weapon) {
		int num = 0;
		switch (weapon) {
		case WeaponType.Single:
			num = 3;
			break;
		case WeaponType.Burst:
			num = 1;
			break;
		default:
			break;
		}
		return num;
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Enemy") {
			_isAlive = false;
			_gc.isPlay = false;
			_gc.state = PlayState.End;
			other.gameObject.SetActive (false);
		}
	}

	public void ScoreUpdate() {
		_scoreText.text = _score.ToString ();
	}
}