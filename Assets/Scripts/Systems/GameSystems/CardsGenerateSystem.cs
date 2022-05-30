using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class CardsGenerateSystem : BaseGameSystem
{
    public enum CardGenerateModes { Heroes, Items, HeroesAndItems }

    [SerializeField] private TMP_InputField _seedInput;
    [SerializeField] private TMP_InputField _cardsCountInput;
    [SerializeField] private TMP_InputField _currentSeedOutput;
    [SerializeField] private CardBase _cardPrefab;
    [SerializeField] private Transform _contentParent;
    [SerializeField] private int _cardCount;
    [SerializeField] private Image _choisedCard;

    [SerializeField] private List<CardBase> _cards = new List<CardBase>();
    [SerializeField] private List<int> _randNumbers = new List<int>();

    [SerializeField] private Random.State _seed;

    [SerializeField] private CardGenerateModes _cardGenerateMode = CardGenerateModes.Heroes;

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
        else if(int.Parse(_cardsCountInput.text) > 0)
            _cardCount = int.Parse(_cardsCountInput.text);
        
        if(_cardCount > data.heroesData.heroesSprites.Length)
        {
            Debug.LogError($"You want spawn cards more than you have sprites");
            data.matchData.state.Value = MatchData.State.EndGame;
            return;
        }

        MainCardIsLocked = false;

        if (!string.IsNullOrEmpty(_seedInput.text)) Random.InitState(int.Parse(_seedInput.text));
        _seed = Random.state;
        _currentSeedOutput.text = Random.seed.ToString();

        for (int i = 0; i < _cardCount; i++)
        {
            Sprite heroData;
            if(_cardGenerateMode == CardGenerateModes.Heroes)
                heroData = data.heroesData.heroesSprites[GetRandomNonRepetitiveNumber(0, data.heroesData.heroesSprites.Length)];
            else if (_cardGenerateMode == CardGenerateModes.Items)
                heroData = data.heroesData.itemsSprites[GetRandomNonRepetitiveNumber(0, data.heroesData.itemsSprites.Length)];
            else
            {
                List<Sprite> heroesAndItems = new List<Sprite>();
                heroesAndItems.AddRange(data.heroesData.heroesSprites);
                heroesAndItems.AddRange(data.heroesData.itemsSprites);

                heroData = heroesAndItems[GetRandomNonRepetitiveNumber(0, heroesAndItems.Count)];
            }

            var card = Instantiate(_cardPrefab, _contentParent);
            card.SetData(this, heroData);
            _cards.Add(card);
        }

        ChoiseRandomCard();

        data.matchData.state.Value = MatchData.State.Game;
    }

    public void SetGenerateMode(CardGenerateModes status = CardGenerateModes.Heroes)
    {
        _cardGenerateMode = status;
    }

    public void ChoiseRandomCard()
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

    public override void RestartGame()
    {
        ClearGame();
        Initialize();
    }
}
