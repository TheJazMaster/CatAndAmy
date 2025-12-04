using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using Shockah.Kokoro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TheJazMaster.CatAndAmy.Artifacts;
using TheJazMaster.CatAndAmy.Backgrounds;
using TheJazMaster.CatAndAmy.Cards;
using TheJazMaster.CatAndAmy.Features;

namespace TheJazMaster.CatAndAmy;

public sealed partial class ModEntry : SimpleMod {
    internal static ModEntry Instance { get; private set; } = null!;

    internal Harmony Harmony { get; }
	internal IKokoroApi.IV2 KokoroApi { get; }
	internal IMoreDifficultiesApi? MoreDifficultiesApi { get; }
	internal IDuoArtifactsApi? DuoArtifactsApi { get; }
    internal LocalDB LocalDB { get; private set; } = null!;

    public const string AMY_COLOR = "fc9ee9";
	public const string CAT_COLOR = "4e3ed1";//"4530bb";


    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
	internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }

    internal IPlayableCharacterEntryV2 CatAndAmyCharacter { get; }
	internal INonPlayableCharacterEntryV2 QuestionMarkCharacter { get; }

    internal IDeckEntry CatAndAmyDeck { get; }

	internal Status LoversStatus { get; }
	internal Status SwitchesStatus { get; }
    internal Status DevotionStatus { get; }
    internal Status KissesStatus { get; }
	internal Status KissesAStatus { get; }
	internal Status ReflexStatus { get; }
	internal Status TempReflexStatus { get; }
    internal Status TempStrafeStatus => KokoroApi.TempStrafeStatus.Status;
    internal Status LeadOnStatus { get; }

    internal Spr GirliesFrame { get; }
    internal Spr GirliesCardBorder { get; }

	internal Spr FloppableFlippedIcon { get; }
    internal Spr PairIcon { get; }
    internal Spr PairTopIcon { get; }
    internal Spr PairBottomIcon { get; }
	internal Spr PairHandIcon { get; }
	
	internal Spr RhodeBG { get; }
	internal Spr RhodeFG { get; }


	internal static IReadOnlyList<Type> CommonCardTypes { get; } = [
		typeof(Grapple),
		typeof(Toss),
		typeof(DoubleStrike),
		typeof(Backhand),
		typeof(LeadOn),
		typeof(Dropkick),
		typeof(Cruise),
		typeof(BackBreaker),
        typeof(CheckIn),
	];

	internal static IReadOnlyList<Type> UncommonCardTypes { get; } = [
		typeof(BaitAndSwitch),
		typeof(ButchAndFemme),
		typeof(Commitment),
		typeof(SmashAndGrab),
		typeof(YouHaul),
		typeof(Suplex),
		typeof(Powerlifting),
	];

	internal static IReadOnlyList<Type> RareCardTypes { get; } = [
		typeof(DoubleSwitch),
		typeof(Kisses),
		typeof(UndyingLove),
		typeof(TrackStarPartyQueen),
		typeof(BootBlack),
	];

	internal static IReadOnlyList<Type> SecretCardTypes { get; } = [
		typeof(Kiss),
		typeof(StarCrossedLovers),
	];

    internal static IEnumerable<Type> AllCardTypes
		=> SecretCardTypes
			.Concat(CommonCardTypes)
			.Concat(UncommonCardTypes)
			.Concat(RareCardTypes).AddItem(typeof(CatAndAmyExe));

    internal static IReadOnlyList<Type> CommonArtifacts { get; } = [
		typeof(OverwhelmingAttraction),
		typeof(HeartShapedCookies),
		typeof(WorkoutRoutine),
		typeof(SentimentalLocket),
	];

	internal static IReadOnlyList<Type> BossArtifacts { get; } = [
		typeof(LoveLetters),
		typeof(PowerCouple)
	];

	internal static IReadOnlyList<Type> DuoArtifacts { get; } = [
		typeof(LovesProtection),
		typeof(BeatEmUp),
		typeof(PerseveringPartnership),
		typeof(LoveBomb),
		typeof(FlamesOfPassion),
		typeof(RainbowPride),
		typeof(CrystalGirlies),
		typeof(AmySandwich)
	];

	internal static IEnumerable<Type> AllArtifactTypes
		=> CommonArtifacts.Concat(BossArtifacts).Concat(DuoArtifacts);

    
    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
	{
		Instance = this;
		Harmony = new(package.Manifest.UniqueName);
		MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");
		KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;
		DuoArtifactsApi = helper.ModRegistry.GetApi<IDuoArtifactsApi>("Shockah.DuoArtifacts");

		AnyLocalizations = new JsonLocalizationProvider(
			tokenExtractor: new SimpleLocalizationTokenExtractor(),
			localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"I18n/{locale}.json").OpenRead()
		);
		Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
			new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
		);

		PairManager.Initialize(package, helper);
		_ = new StatusManager();

		GirliesFrame = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Character/Panel.png")).Sprite;
        GirliesCardBorder = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Character/CardBorder.png")).Sprite;

        LeadOnStatus = helper.Content.Statuses.RegisterStatus("LeadOn", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/LeadOn.png")).Sprite,
				color = new("BE00A0"),
				isGood = true,
				affectedByTimestop = true
			},
			Name = AnyLocalizations.Bind(["status", "LeadOn", "name"]).Localize,
			Description = AnyLocalizations.Bind(["status", "LeadOn", "description"]).Localize
		}).Status;

        ReflexStatus = helper.Content.Statuses.RegisterStatus("Reflex", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/Reflex.png")).Sprite,
				color = new("FF373D"),
				isGood = true
			},
			Name = AnyLocalizations.Bind(["status", "Reflex", "name"]).Localize,
			Description = AnyLocalizations.Bind(["status", "Reflex", "description"]).Localize
		}).Status;

        TempReflexStatus = helper.Content.Statuses.RegisterStatus("TempReflex", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/TempReflex.png")).Sprite,
				color = new("E517C6"),
				isGood = true,
				affectedByTimestop = true
			},
			Name = AnyLocalizations.Bind(["status", "TempReflex", "name"]).Localize,
			Description = AnyLocalizations.Bind(["status", "TempReflex", "description"]).Localize
		}).Status;

        KissesStatus = helper.Content.Statuses.RegisterStatus("Kisses", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/Kisses.png")).Sprite,
				color = new("FFFFFF"),
				isGood = true
			},
			Name = AnyLocalizations.Bind(["status", "Kisses", "name"]).Localize,
			Description = AnyLocalizations.Bind(["status", "Kisses", "description"]).Localize
		}).Status;

        KissesAStatus = helper.Content.Statuses.RegisterStatus("KissesA", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/KissesA.png")).Sprite,
				color = new("FFFD4E"),
				isGood = true
			},
			Name = AnyLocalizations.Bind(["status", "KissesA", "name"]).Localize,
			Description = AnyLocalizations.Bind(["status", "KissesA", "description"]).Localize
		}).Status;

        LoversStatus = helper.Content.Statuses.RegisterStatus("Lovers", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/Lovers.png")).Sprite,
				color = new("FF89C4"),
				isGood = true,
				affectedByTimestop = true
			},
			Name = AnyLocalizations.Bind(["status", "Lovers", "name"]).Localize,
			Description = AnyLocalizations.Bind(["status", "Lovers", "description"]).Localize
		}).Status;

        DevotionStatus = helper.Content.Statuses.RegisterStatus("Devotion", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/Devotion.png")).Sprite,
				color = new("FF89C4"),
				isGood = true
			},
			Name = AnyLocalizations.Bind(["status", "Devotion", "name"]).Localize,
			Description = AnyLocalizations.Bind(["status", "Devotion", "description"]).Localize
		}).Status;

        SwitchesStatus = helper.Content.Statuses.RegisterStatus("Switches", new()
		{
			Definition = new()
			{
				icon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/Switches.png")).Sprite,
				color = new("294865"),
				isGood = true,
				affectedByTimestop = true
			},
			Name = AnyLocalizations.Bind(["status", "Switches", "name"]).Localize,
			Description = AnyLocalizations.Bind(["status", "Switches", "description"]).Localize
		}).Status;

		FloppableFlippedIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/FloppableFlipped.png")).Sprite;
		PairIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/Pair.png")).Sprite;
		PairTopIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/PairTop.png")).Sprite;
		PairBottomIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/PairBottom.png")).Sprite;
		PairHandIcon = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Icons/PairHand.png")).Sprite;

		RhodeBG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Memories/RhodeBG.png")).Sprite;
		RhodeFG = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Memories/RhodeFG.png")).Sprite;

		CatAndAmyDeck = helper.Content.Decks.RegisterDeck("CatAndAmy", new()
		{
			Definition = new() { color = new Color("442DB9"), titleColor = Colors.white },
			DefaultCardArt = StableSpr.cards_colorless,
			BorderSprite = GirliesCardBorder,
			Name = AnyLocalizations.Bind(["character", "name"]).Localize
		});

        foreach (var cardType in AllCardTypes)
			AccessTools.DeclaredMethod(cardType, nameof(IRegisterableCard.Register))?.Invoke(null, [helper, package]);
		foreach (var artifactType in AllArtifactTypes)
			AccessTools.DeclaredMethod(artifactType, nameof(IRegisterableArtifact.Register))?.Invoke(null, [helper, package]);

		MoreDifficultiesApi?.RegisterAltStarters(CatAndAmyDeck.Deck, new StarterDeck {
            cards = {
                new DoubleStrike(),
                new Backhand()
            }
        });

        Dictionary<string, CharacterAnimationConfigurationV2> animations = RegisterAllAnimations();
        CatAndAmyCharacter = helper.Content.Characters.V2.RegisterPlayableCharacter("CatAndAmy", new()
		{
			Deck = CatAndAmyDeck.Deck,
			Description = AnyLocalizations.Bind(["character", "description"]).Localize,
			BorderSprite = GirliesFrame,
			Starters = new StarterDeck {
				cards = [ new Grapple(), new Toss() ],
			},
			ExeCardType = typeof(CatAndAmyExe),
			NeutralAnimation = animations["neutral"],
			MiniAnimation = animations["mini"]
		});
        // example line: "I'm Cat!\n<a=a>And I'm Amy!\n<a=b>We're a team!"
		QuestionMarkCharacter = helper.Content.Characters.V2.RegisterNonPlayableCharacter("QuestionMark", new NonPlayableCharacterConfigurationV2()
		{
			CharacterType = "questionmark",
			Name = AnyLocalizations.Bind(["character", "questionmark"]).Localize
		});
		helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2()
		{
			CharacterType = QuestionMarkCharacter.CharacterType,
			LoopTag = "neutral",
			Frames = animations["painglowglitch"].Frames
		});

        helper.Content.Cards.OnGetDynamicInnateCardTraitOverrides += (_, data) => {
			State state = data.State;
			if (state.ship.Get(SwitchesStatus) > 0 && data.InnateTraits.Contains(PairManager.PairTrait)) {
				data.SetOverride(PairManager.FloppableTrait, true);
			}
		};

		Vault.charsWithLore.Add(CatAndAmyDeck.Deck);
		BGRunWin.charFullBodySprites.Add(CatAndAmyDeck.Deck, helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("Sprites/Memories/fullBody.png")).Sprite);
		
        StoryDialogue.Initialize(package, helper);
        CombatDialogue.Initialize(package, helper);
		EventDialogue.Initialize(package, helper);

        helper.Events.OnModLoadPhaseFinished += (_, phase) =>
        {
            if (phase == ModLoadPhase.AfterDbInit)
            {
            	LocalDB = new(helper, package);
                DB.backgrounds.Add("CatAndAmy_BGRhode", typeof(BGRhode));
            }
        };
		helper.Events.OnLoadStringsForLocale += (_, thing) =>
        {
            foreach (KeyValuePair<string, string> entry in LocalDB.GetLocalizationResults())
            {
                thing.Localizations[entry.Key] = entry.Value;
            }
        };

		Harmony.PatchAll();
    }

	private Dictionary<string, CharacterAnimationConfigurationV2> RegisterAllAnimations() {
        Dictionary<string, CharacterAnimationConfigurationV2> ret = [];
        foreach (CharacterAnimationConfigurationV2 anim in Package.PackageRoot.GetRelativeDirectory("Sprites/Character").Directories
			.SelectMany(d => d.Directories)
            .SelectMany(RegisterAnimations)) {
            ret.Add(anim.LoopTag, anim);
        }
        return ret;
    }

	private List<CharacterAnimationConfigurationV2> RegisterAnimations(IDirectoryInfo dir) {
		if (!MyRegex3().Match(dir.Name).Success) return [];
        if (!dir.Directories.Any()) return [
            RegisterAnimation(dir, SanitizeName(dir.Name))
        ];
        return dir.Directories.Select(d => RegisterAnimation(d, SanitizeName(d.Name))).ToList();
    }

    private CharacterAnimationConfigurationV2 RegisterAnimation(IDirectoryInfo dir, string name)
    {
        string n = RemoveTalkingSuffix(dir.Name);
        var frames = Enumerable.Range(1, 10)
                .Select(i => dir.GetRelativeFile($"{n}-{i}.png"))
                .Where(f => f.Exists)
                .Select(f => Helper.Content.Sprites.RegisterSprite(f).Sprite)
                .ToList();
		if (frames.Count > 1) frames.Add(frames[0]);
        return Helper.Content.Characters.V2.RegisterCharacterAnimation("CatAndAmy_" + name, new()
        {
            CharacterType = CatAndAmyDeck.Deck.Key(),
            LoopTag = name,
			Frames = frames
        }).Configuration;
    }

    private static string RemoveTalkingSuffix(string name) {
        name = name.ToLower();
        name = MyRegex().Replace(name, "");
		name = MyRegex1().Replace(name, "");
		name = MyRegex2().Replace(name, "");
        return name;
    }
	
	private static string SanitizeName(string name) {
        name = name.ToLower();
        name = MyRegex().Replace(name, "_amy");
		name = MyRegex1().Replace(name, "_cat");
		name = MyRegex2().Replace(name, "");
        name = name.Replace("v1", "");
        return name;
    }
    [GeneratedRegex("amytalking")]
    private static partial Regex MyRegex();
    [GeneratedRegex("cattalking")]
    private static partial Regex MyRegex1();
    [GeneratedRegex("bothtalking")]
    private static partial Regex MyRegex2();


	public override object? GetApi(IModManifest requestingMod)
		=> new ApiImplementation();
    [GeneratedRegex("V\\d")]
    private static partial Regex MyRegex3();
}