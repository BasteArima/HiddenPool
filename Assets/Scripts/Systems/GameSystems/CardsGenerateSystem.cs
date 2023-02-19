using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class CardsGenerateSystem : BaseGameSystem
{
    [SerializeField] private CardBase _cardPrefab;
    [SerializeField] private Transform _contentParent;
    
    [SerializeField] private Image _choisedCard;
    
    [SerializeField] private TMP_InputField _seedInput;
    [SerializeField] private TMP_InputField _cardsCountInput;
    [SerializeField] private TMP_InputField _currentSeedOutput;

    [SerializeField] private Random.State _seed;

    private List<CardBase> _cards = new List<CardBase>();
    private List<int> _randNumbers = new List<int>();
    private int _cardCount = 40;
    
    public string Seed
    {
        get => _seedInput.text;
        set => _seedInput.text = value;
    }

    public string CardsCount
    {
        get => _cardsCountInput.text;
        set => _cardsCountInput.text = value;
    }

    public bool MainCardIsLocked { get; private set; }

    public override void Init(AppData data)
    {
        base.Init(data);
        SetObservables();
    }

    private void SetObservables()
    {
        data.matchData.state
            .Where(x => x == MatchData.State.EndGame)
            .Subscribe(_ => EndGame());
    }

    public override void Initialize()
    {
        if (string.IsNullOrEmpty(_cardsCountInput.text))
            _cardsCountInput.text = _cardCount.ToString();
        else if (int.Parse(_cardsCountInput.text) > 0)
            _cardCount = int.Parse(_cardsCountInput.text);

        if (_cardCount > data.packsData.heroesSprites.Length)
        {
            Debug.Log($"Player want spawn cards more than sprites");
            _cardCount = data.packsData.heroesSprites.Length;
            _cardsCountInput.text = _cardCount.ToString();
        }

        MainCardIsLocked = false;

        if (!string.IsNullOrEmpty(_seedInput.text))
            Random.InitState(int.Parse(_seedInput.text));
        
        _seed = Random.state;
        _currentSeedOutput.text = Random.seed.ToString();

        for (int i = 0; i < _cardCount; i++)
        {
            var heroData = data.packsData.heroesSprites[
                    GetRandomNonRepetitiveNumber(0, data.packsData.heroesSprites.Length)];

            var card = Instantiate(_cardPrefab, _contentParent);
            card.SetData(this, heroData);
            _cards.Add(card);
        }

        ChoiceRandomCard();

        data.matchData.state.Value = MatchData.State.Game;
    }

    public void ChoiceRandomCard()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        _choisedCard.sprite = _cards[Random.Range(0, _cards.Count)].CardSprite;
    }

    public void SetMainCard(Sprite sprite)
    {
        if (MainCardIsLocked) return;

        _choisedCard.sprite = sprite;
    }

    public void RefreshCards()
    {
        foreach (var card in _cards)
            card.ClearCard();
    }

    public void ToggleLockMainCard()
    {
        MainCardIsLocked = !MainCardIsLocked;
    }

    private int GetRandomNonRepetitiveNumber(int min, int max)
    {
        int randNumber;

        do
        {
            randNumber = Random.Range(min, max);
        } while (_randNumbers.Contains(randNumber));

        _randNumbers.Add(randNumber);
        return randNumber;
    }

    public override void EndGame()
    {
        ClearGame();
        data.matchData.state.Value = MatchData.State.MainMenu;
    }

    private void ClearGame()
    {
        foreach (var card in _cards)
            Destroy(card.gameObject);

        _cards.Clear();
        _randNumbers.Clear();
    }

    public void RestartGameByClient()
    {
        ClearGame();
        Initialize();
    }

    public override void RestartGame()
    {
        ClearGame();
        Initialize();
        
        
        
        //if(FirebaseController.Instance.Initialized)
        //    FirebaseController.Instance.UpdateRoomValuesOnDb(_seedInput.text, _cardCount);
    }
}