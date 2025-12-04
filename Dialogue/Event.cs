
using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.CatAndAmy;

static class EventDialogue {

    public static void Initialize(IPluginPackage<IModManifest> package, IModHelper helper) {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>()
        {
            {"CatAndAmy_Shopkeeper_0", new() {
                type = NodeType.@event,
                lookup = [ "shopBefore" ],
                allPresent = [ "cna" ],
                bg = "BGShop",
                once = true,
                priority = true,
                dialogue = [
                    new("cleo", "Woah! You two look different! Where are you from?", true),
                    new("cna", "Earth."),
                    new("cleo", "Wild!", true),
                    new(new Jump { key = "NewShop" })
                ]
            }},
            {"CatAndAmy_Shopkeeper_1", new() {
                type = NodeType.@event,
                lookup = [ "shopBefore" ],
                allPresent = [ "cna" ],
                bg = "BGShop",
                dialogue = [
                    new("cleo", "Meowdy!", true),
                    new("cna", "blushingv2", "<a=c>Look, Amy! Another cat girl! You're right at home here!"),
                    new(new Jump { key = "NewShop" })
                ]
            }},
            {"CatAndAmy_Shopkeeper_2", new() {
                type = NodeType.@event,
                lookup = [ "shopBefore" ],
                allPresent = [ "cna" ],
                bg = "BGShop",
                dialogue = [
                    new("cleo", "Meowdy!", true),
                    new("cna", "affection", "Meowdy!"),
                    new("cat", "squint", "Not another one."),
                    new(new Jump { key = "NewShop" })
                ]
            }},
            {"CatAndAmy_Shopkeeper_3", new() {
                type = NodeType.@event,
                lookup = [ "shopBefore" ],
                allPresent = [ "cna" ],
                bg = "BGShop",
                dialogue = [
                    new("cleo", "Welcome back, Earthlings!", true),
                    new("cna", "Thanks!"),
                    new(new Jump { key = "NewShop" })
                ]
            }},
            {"CatAndAmy_Shopkeeper_4", new() {
                type = NodeType.@event,
                lookup = [ "shopBefore" ],
                allPresent = [ "cna" ],
                bg = "BGShop",
                dialogue = [
                    new("cleo", "Heya, you two!", true),
                    new("cna", "Hi!"),
                    new(new Jump { key = "NewShop" })
                ]
            }},

            {"LoseCharacterCard", new() {
                edit = [
                    new(EMod.countFromEnd, 0, "cna", "serious", "We aren't going out like this! Act fast!"),
                    new(EMod.countFromEnd, 0, "cna", "concernv2", "<a=c>Hold on, Amy! We'll make it through this!"),
                    new(EMod.countFromEnd, 0, "cna", "concernv2", "<a=a>Cat! I'm not ready to lose you yet!")
                ]
            }},
            {$"LoseCharacterCard_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}", new() {
                type = NodeType.@event,
                oncePerRun = true,
                bg = "BGSupernova",
                dialogue = [
                    new("cna", "annoyedv2", "Ow. But better that than us."),
                ]
            }},

            {$"ChoiceCardRewardOfYourColorChoice_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}", new() {
                type = NodeType.@event,
                oncePerRun = true,
                bg = "BGBootSequence",
                dialogue = [
                    new([
                        new("cna", "squint", "Huh... Did we always know that?"),
                        new("cna", "squint", "<a=c>Strange. I suddenly have a skill that wasn't there before.\n<a=a>Me too."),
                    ]),
                    new("cat", "Energy readings are back to normal."),
                ]
            }},
            
            {"GrandmaShop", new(){
                edit = [
                    new(EMod.countFromStart, 0, "cna", "pumped", "Strawberry mousse!"),
                    new(EMod.countFromStart, 0, "cna", "affectionv4", "Valentines' chocolates!"),
                    new(EMod.countFromStart, 0, "cna", "Blueberry-apple pie?")
                ]
            }},
            
            {"Soggins_Infinite", new(){
                edit = [
                    new(EMod.countFromStart, 0, "cna", "agreeingv2", "This poor frog seems more lost than us."),
                    new(EMod.countFromStart, 0, "cna", "agreeingv2", "Looks like we're not the only ones having a hard time."),
                    new(EMod.countFromStart, 0, "cna", "Is he gonna be alright?")
                ]
            }},
            
            {"Sasha_2_Multi_2", new(){
                edit = [
                    new(EMod.countFromStart, 0, "cna", "pumped", "Which ones?"),
                    new(EMod.countFromStart, 0, "cna", "pumped", "Heck yeah! Sports!"),
                    new(EMod.countFromStart, 0, "cna", "pumped", "Where do we sign up?"),
                ]
            }},
            
            {"DraculaTime", new(){
                edit = [
                    new(EMod.countFromStart, 0, "cna", "annoyed", "You know, I'm not surprised."),
                    new(EMod.countFromStart, 0, "cna", "annoyed", "Of course. Of all things from Earth, Dracula."),
                    new(EMod.countFromStart, 0, "cna", "annoyedv2", "Dracula. Like. That Dracula?"),
                    new(EMod.countFromStart, 0, "cna", "neutralv2", "You know, given everything, this doesn't feel so shocking."),
                ]
            }},

            // {"CrystallizedFriendEvent", new(){
            //     edit = [new("8383e940", AmWeth, "plead", "No...")]
            // }},
            // {$"CrystallizedFriendEvent_{AmWeth}", new(){
            //     type = NodeType.@event,
            //     oncePerRun = true,
            //     bg = "BGCrystalizedFriend",
            //     allPresent = [AmWeth],
            //     dialogue = [
            //         new(new Wait{secs = 1.5}),
            //         new(AmWeth, "yay", "Hello! I'm ready to adventure!")
            //     ]
            // }},
        });
    }
}