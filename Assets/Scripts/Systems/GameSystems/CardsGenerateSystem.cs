using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardsGenerateSystem : BaseGameSystem
{
    [SerializeField] private TMP_InputField _seedInput;
    [SerializeField] private TMP_InputField _currentSeedOutput;
    [SerializeField] private CardBase _cardPrefab;
    [SerializeField] private Transform _contentParent;
    [SerializeField] private int _cardCount;
    [SerializeField] private Image _choisedCard;

    [SerializeField] private List<CardBase> _cards = new List<CardBase>();
    [SerializeField] private List<int> _randNumbers = new List<int>();

    [SerializeField] private Random.State _seed;

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

    /// <summary>
    /// Должен работать на хосте/сервере одинаково для всех создавать героев
    /// </summary>
    public override void Initialize()
    {
        if(!string.IsNullOrEmpty(_seedInput.text)) Random.InitState(int.Parse(_seedInput.text));
        _seed = Random.state;
        _currentSeedOutput.text = Random.seed.ToString();

        for (int i = 0; i < _cardCount; i++)
        {
            var heroData = data.heroesData.heroesSprites[GetRandomNonRepetitiveNumber(0, data.heroesData.heroesSprites.Length)];
            //var heroData = data.heroesData.heroes[Random.Range(0,data.heroesData.heroes.Length+1)];
            var card = Instantiate(_cardPrefab, _contentParent);
            //card.SetData(heroData.cardImage, heroData.name, heroData.attribute);
            card.SetData(this, heroData);
            _cards.Add(card);
        }

        ChoiseRandomCard();

        data.matchData.state.Value = MatchData.State.Game;
    }

    /// <summary>
    /// Только на клиенте, возможно стоит вынести из инит метода и вынести в отдельный стейт
    /// </summary>
    public void ChoiseRandomCard()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        _choisedCard.sprite = _cards[Random.Range(0, _cards.Count)].CardSprite;
    }

    public void SetMainCard(Sprite sprite)
    {
        _choisedCard.sprite = sprite;
    }

    public void RefreshCards()
    {
        foreach (var card in _cards)
            card.ClearCard();
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

    public override void RestartGame()
    {
        ClearGame();
        Initialize();
    }
}
