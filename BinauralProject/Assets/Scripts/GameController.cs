using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public delegate void onComplete();

[System.Serializable]
public enum Difficulty {
	Easy = 0,
	Normal,
	Hard,
	Extra
}

public class GameController : MonoBehaviour {
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
	private int _eCount;
	private List<Enemy> _enemys = new List<Enemy>();
	private Enemy _boss;
	private Timer _timer;
	private PlayerController _pc;
	private Difficulty _diffcult;
	private int _diffNum;
	void Start() {
		_pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		_timer = GetComponent<Timer>();
		_diffcult = Difficulty.Easy;
		_diffNum = (int)_diffcult;
		CreateEnemy();
		StartCoroutine(GenerateEnemy());
	}

	// Update is called once per frame
	void Update() {
	}

	void CreateEnemy() {
		for(int i = 0; i < _eMax; i++) {
			var enemy = Instantiate(_eInstance, _eParent);
			enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
			_enemys.Add(enemy.GetComponent<Enemy>());
			enemy.SetActive(false);
		}
		var boss = Instantiate(_eInstance, _eParent);
		boss.transform.rotation = Quaternion.Euler(0, 180, 0);
		_boss = boss.GetComponent<Enemy>();
		boss.SetActive(false);
		_eCount = 0;
	}

	IEnumerator GenerateEnemy() {
		while(_isPlay) {
			if(_eCount >= _eMax)
				continue;
			SetEnemyType();
			yield return new WaitForSecondsRealtime(7f);
		}
		yield break;
	}

	void SetEnemyType() {
		_enemys[_eCount].gameObject.SetActive(true);
		if(_diffcult != Difficulty.Extra) {
			var rNum = Random.Range(0, _diffNum + 1);
			_enemys[_eCount].eType = (EnemyType)rNum;
		}
		else
			_enemys[_eCount].eType = EnemyType.Hard;
		_enemys[_eCount].EnemyInit();
		_eCount++;
	}
	/*
	public void GenerateBoss(onComplete callBack) {
		_boss.gameObject.SetActive(true);
		_boss.eType = (EnemyType)4;
		_boss.EnemyInit();
		callBack();
	}
	*/
	public void GenerateBoss() {
		_boss.gameObject.SetActive(true);
		_boss.eType = EnemyType.Boss;
		_boss.EnemyInit();
	}
}
