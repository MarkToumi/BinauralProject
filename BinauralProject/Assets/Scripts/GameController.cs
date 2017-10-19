using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public delegate void onComplete ();

[System.Serializable]
public enum Difficulty {
	Easy = 0,
	Normal,
	Hard,
	Extra
}

[System.Serializable]
public enum PlayState {
	Start = 0,
	Play,
	End
}

public class GameController : MonoBehaviour {
	private PlayState _state;

	public PlayState state {
		get { return _state; }
		set { _state = value; }
	}

	private bool _isPlay = true;

	public bool isPlay {
		get { return _isPlay; }
		set { _isPlay = value; }
	}

	[SerializeField]
	private Transform _eParent;
	[SerializeField]
	private GameObject _eInstance;
	[SerializeField]
	private int _eMax;
	[SerializeField]
	private float _radius;
	private int _eCount;
	private List<Enemy> _enemys = new List<Enemy>();
	private Enemy _boss;
	private Timer _timer;
	private PlayerController _pc;
	private Difficulty _diffcult;
	private int _diffNum;
	[SerializeField]
	private List<Button> _buttons = new List<Button>();

	void Start() {
		_pc = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		_state = PlayState.Start;
		_timer = GetComponent<Timer> ();
		_diffcult = Difficulty.Easy;
		_diffNum = (int)_diffcult;
		CreateEnemy ();
	}

	// Update is called once per frame
	void Update() {
		if (_state == PlayState.End) {
			if (Input.GetButtonDown ("Fire1"))
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	void CreateEnemy() {
		for (int i = 0; i < _eMax; i++) {
			var enemy = Instantiate (_eInstance, _eParent);
			enemy.transform.rotation = Quaternion.Euler (0, 180, 0);
			_enemys.Add (enemy.GetComponent<Enemy> ());
			enemy.SetActive (false);
		}
		var boss = Instantiate (_eInstance, _eParent);
		boss.transform.rotation = Quaternion.Euler (0, 180, 0);
		_boss = boss.GetComponent<Enemy> ();
		boss.SetActive (false);
		_eCount = 0;
	}

	IEnumerator GenerateEnemy() {
		while (_isPlay) {
			if (_eCount >= _eMax)
				continue;
			else
				SetEnemyType ();
			yield return new WaitForSecondsRealtime(7f);
		}
		yield break;
	}

	void SetEnemyType() {
		_enemys [_eCount].gameObject.SetActive (true);
		if (_diffcult != Difficulty.Extra) {
			var rNum = Random.Range (0, _diffNum + 1);
			_enemys [_eCount].eType = (EnemyType)rNum;
		}
		else
			_enemys [_eCount].eType = EnemyType.Hard;
		var rPos = (Random.Range(0.0f, 180.0f) - 90.0f) * Mathf.Deg2Rad;
		var nowPos = transform.position;
		nowPos.x += _radius * Mathf.Cos(rPos);
		nowPos.z += _radius * Mathf.Sin(rPos);
		_enemys[_eCount].gameObject.transform.position = nowPos;
		_enemys [_eCount].EnemyInit ();
		_eCount++;
	}

	public void GenerateBoss() {
		_boss.gameObject.SetActive (true);
		_boss.eType = EnemyType.Boss;
		_boss.EnemyInit ();
	}

	public void SelectDiffcult(int d) {
		_diffcult = (Difficulty)d;
		foreach (var b in _buttons)
			b.gameObject.SetActive (false);
		_state = PlayState.Play;
		StartCoroutine (GenerateEnemy ());
	}
}

// 拡張クラス
[System.Serializable]
public static class EnumEx {
	public static int Size(this WeaponType weapon) {
		return System.Enum.GetValues (typeof(WeaponType)).Length;
	}

	public static int Size(this Difficulty diff) {
		return System.Enum.GetValues (typeof(Difficulty)).Length;
	}

	public static int Size(this PlayState state) {
		return System.Enum.GetValues (typeof(PlayState)).Length;
	}
}