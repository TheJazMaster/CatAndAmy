using System;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.CatAndAmy;

internal interface IRegisterableCard
{
	static ICardEntry Register(Type type, Rarity rarity, IModHelper helper, IPluginPackage<IModManifest> package, out string name, Deck? deck = null, bool dontOffer = false) {
		name = type.Name;
		return helper.Content.Cards.RegisterCard(name, new()
		{
			CardType = type,
			Meta = new()
			{
				deck = deck ?? ModEntry.Instance.CatAndAmyDeck.Deck,
				rarity = rarity,
				upgradesTo = [Upgrade.A, Upgrade.B],
				dontOffer = dontOffer
			},
			Art = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Cards/{name}.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", name, "name"]).Localize
		});
	}
	static ICardEntry Register(Type type, Rarity rarity, IModHelper helper, IPluginPackage<IModManifest> package, out string name, out Spr amySprite, out Spr catSprite, Deck? deck = null) {
		name = type.Name[..^4];
		amySprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Cards/{name}Amy.png")).Sprite;
		catSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Cards/{name}Cat.png")).Sprite;
		return helper.Content.Cards.RegisterCard(name, new()
		{
			CardType = type,
			Meta = new()
			{
				deck = deck ?? ModEntry.Instance.CatAndAmyDeck.Deck,
				rarity = rarity,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Art = amySprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", name, "name"]).Localize
		});
	}
	static ICardEntry Register(Type type, Rarity rarity, IModHelper helper, IPluginPackage<IModManifest> package, out string name, out Spr bothSprite, out Spr amySprite, out Spr catSprite, Deck? deck = null) {
		name = type.Name;
		bothSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Cards/{name}Duo.png")).Sprite;
		amySprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Cards/{name}Amy.png")).Sprite;
		catSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Cards/{name}Cat.png")).Sprite;
		return helper.Content.Cards.RegisterCard(name, new()
		{
			CardType = type,
			Meta = new()
			{
				deck = deck ?? ModEntry.Instance.CatAndAmyDeck.Deck,
				rarity = rarity,
				upgradesTo = [Upgrade.A, Upgrade.B]
			},
			Art = bothSprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", name, "name"]).Localize
		});
	}

	static abstract void Register(IModHelper helper, IPluginPackage<IModManifest> package);
}

internal interface IRegisterableArtifact
{
	static IArtifactEntry Register(Type type, ArtifactPool[] pools, IModHelper helper, IPluginPackage<IModManifest> package, out string name, Deck? deck = null, bool unremovable = false) {
		name = type.Name;
		return helper.Content.Artifacts.RegisterArtifact(name, new()
		{
			ArtifactType = type,
			Meta = new()
			{
				owner = deck ?? ModEntry.Instance.CatAndAmyDeck.Deck,
				pools = pools,
				unremovable = unremovable
			},
			Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Artifacts/{name}.png")).Sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", name, "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", name, "description"]).Localize
		});
	}
	static IArtifactEntry Register(Type type, ArtifactPool[] pools, IModHelper helper, IPluginPackage<IModManifest> package, out string name, out Spr activeSpr, out Spr inactiveSpr, Deck? deck = null, bool unremovable = false) {
		name = type.Name;
		activeSpr = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Artifacts/{name}.png")).Sprite;
		inactiveSpr = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/Artifacts/{name}Disabled.png")).Sprite;
		return helper.Content.Artifacts.RegisterArtifact(name, new()
		{
			ArtifactType = type,
			Meta = new()
			{
				owner = deck ?? ModEntry.Instance.CatAndAmyDeck.Deck,
				pools = pools,
				unremovable = unremovable
			},
			Sprite = activeSpr,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", name, "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", name, "description"]).Localize
		});
	}
	static abstract void Register(IModHelper helper, IPluginPackage<IModManifest> package);
}