
using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;
using TheJazMaster.CatAndAmy.Features;

namespace TheJazMaster.CatAndAmy;

static class CombatDialogue {
    private static Deck CnA => ModEntry.Instance.CatAndAmyDeck.Deck;

    public static void Initialize(IPluginPackage<IModManifest> package, IModHelper helper) {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>()
        {
            {"CatAndAmy_FirstCombat", new(){
                type = NodeType.combat,
                allPresent = ["cna"],
                once = true,
                dialogue = [
                    new("cna", "squint", "<a=c>What even IS all this stuff?\n<a=a>One step at a time, babe. We'll figure it out. Somehow."),
                ]
            }},
            {"Crystal_CatAndAmy_1", new(){
                type = NodeType.combat,
                allPresent = ["cna", "crystal"],
                once = true,
                dialogue = [
                    new("cna", "<a=c>Wow, it's gorgeous!\n<a=a>I- I've never seen anything like it!"),
                ]
            }},
            {"JustHitPDA", new DialogueMachine(){
                type = NodeType.combat,
                playerShotJustHit = true,
                allPresent = ["cna"],
                whoDidThat = CnA,
                oncePerCombat = true,
                dialogue = [
                    new([
                        new("cna", "pumpedv2", "<a=c>Ha! Eat it!\n<a=a>Cat, you are so hot."),
                        new("cna", "blushing", "<a=a>Heck yeah!\n<a=c>Amy, I will never stop swooning for you."),
                    ]),
                    new([
                        new("drake", "mad", "Uuuuuugh! GET A ROOM!"),
                        new("drake", "mad", "Ugh. Get a room!"),
                        new("drake", "mad", "Ugh. Keep it to yourselves!"),
                        new("drake", "mad", "Shut up."),
                        new("drake", "mad", "Will you two shut up!"),
                        new("drake", "mad", "We get it! You're in love! SHUT IT!"),
                        new("drake", "mad", "Ugh. I'm gonna puke."),
                        new("drake", "mad", "I'm gonna be sick."),
                        new("drake", "mad", "..."),
                        new("drake", "mad", "Next time I find you two when I'm in the Fireball..."),
                        new("drake", "mad", "Will you two stop! AAAAGH!"),
                        new("drake", "mad", "AAAAGH! SHUT IT!"),
                        new("drake", "mad", "Uuuugh take your love and shove it!"),
                        new("drake", "mad", "Ugh! I wish I could just shove you both out the airlock!"),

                        new("max", "blush", "... Can you cool it with the PDAs?"),
                        new("max", "blush", "Is. Is now the time?"),
                        new("max", "blush", "Is this entirely necessary?"),
                        new("max", "blush", "... Can you two just get a room?"),
                        new("max", "blush", "Am. Am I jealous?"),
                        new("max", "mad", "..."),
                        new("max", "blush", "Is it possible to tone it down a notch?"),
                        new("max", "mad", "I'm not comfortable with this level of affection."),
                        new("max", "mad", "You two need to like, get a room or something."),

                        new("isaac", "shy", "Are all humans like this?"),
                        new("isaac", "squint", "Man. Humans are weird."),
                        new("isaac", "squint", "Are you two like this all the time?"),
                        new("isaac", "squint", "Hey, can we focus?"),
                        new("isaac", "squint", "Let's stay on task here, yeah?"),
                        new("isaac", "squint", "Humans are strange."),
                        new("isaac", "squint", "Humans..."),
                        new("isaac", "shy", "Is there a better time for this than now?"),
                        new("isaac", "shy", "Do you think you two could save this for later?"),
                        new("isaac", "shy", "Why am I uncomfortable right now?"),

                        new("books", "paws", "Hehe!"),
                        new("books", "blush", "You both make me smile!"),
                        new("books", "paws", "I hope I get to be in love like you are someday."),
                        new("books", "paws", "Teehee!"),
                        new("books", "blush", "I wanna be in love like you two are."),
                        new("books", "paws", "Someday..."),
                        new("books", "blush", "Hehe! You're both so pretty!"),
                        new("books", "paws", "I want a girl to love me like this too."),
                        new("books", "paws", "I wanna be in love with someone who's as pretty as you two are."),
                        new("books", "blush", "Cat and Amy sitting in a tree! K. I. S. S. I. N. G."),

                        new("peri", "blush", "..."),
                        new("peri", "I need you two to focus."),
                        new("peri", "Focus, you two."),
                        new("peri", "Eyes on your controls!"),
                        new("peri", "Hey! Eyes on your consoles!"),
                        new("peri", "You two! Focus!"),
                        new("peri", "I need you two to get your heads in the game."),
                        new("peri", "Heads out of the clouds, you two."),
                    ])
                ]
            }.ApplyModData(PairManager.GlobalGirlKey, PairManager.Girl.CAT)},
            {"CatJustHit", new DialogueMachine(){
                type = NodeType.combat,
                playerShotJustHit = true,
                allPresent = ["cna"],
                whoDidThat = CnA,
                dialogue = [
                    new([
                        new("cna", "pumped", "Ha! Got one!"),  
                        new("cna", "pumped", "Yes!"),
                        new("cna", "pumpedv2", "<a=c>Ha! Eat it!\n<a=a>Cat, you are so hot."),
                        new("cna", "blushing", "<a=a>Heck yeah!\n<a=c>Amy, I will never stop swooning for you."),
                        new("cna", "pumped", "Bull's eye!"),
                        new("cna", "pumped", "Take this!"),
                        new("cna", "smug", "<a=c>Amy, if you ever leave me, I'll do that to you.\n<a=a>Roger."),
                        new("cna", "pumped", "<a=a>Wahoo! It's on!\n<a=c>Dork."),

                        new("cna", "pumpedv2", "<a=a>Nice shot, babe!\n<a=c>Thanks!"),
                        new("cna", "agreeingv2", "<a=a>Good one, babe.\n<a=c>Thanks.")
                    ]),
                ]
            }.ApplyModData(PairManager.GlobalGirlKey, PairManager.Girl.CAT)},
            {"AmyJustHit", new DialogueMachine(){
                type = NodeType.combat,
                playerShotJustHit = true,
                allPresent = ["cna"],
                whoDidThat = CnA,
                dialogue = [
                    new([
                        new("cna", "pumped", "Ha! Got one!"),  
                        new("cna", "pumped", "Yes!"),
                        new("cna", "pumpedv2", "<a=c>Ha! Eat it!\n<a=a>Cat, you are so hot."),
                        new("cna", "blushing", "<a=a>Heck yeah!\n<a=c>Amy, I will never stop swooning for you."),
                        new("cna", "pumped", "Bull's eye!"),
                        new("cna", "pumped", "Take this!"),
                        new("cna", "smug", "<a=c>Amy, if you ever leave me, I'll do that to you.\n<a=a>Roger."),
                        new("cna", "pumped", "<a=a>Wahoo! It's on!\n<a=c>Dork."),

                        new("cna", "neutralv2", "<a=c>Good shot, babe.\n<a=a>Thanks."),
                    ]),
                ]
            }.ApplyModData(PairManager.GlobalGirlKey, PairManager.Girl.AMY)},
            {"SomeoneJustHit_CatAndAmy", new DialogueMachine(){
                type = NodeType.combat,
                playerShotJustHit = true,
                minDamageDealtToEnemyThisAction = 6,
                allPresent = ["cna"],
                oncePerCombat = true,
                dialogue = [
                    new([
                        new("cna", "pumped", "Yes!"),
                        new("cna", "pumped", "Bull's eye!"),
                        new("cna", "smug", "<a=c>Amy, if you ever leave me, I'll do that to you.\n<a=a>Roger."),
                        new("cna", "pumped", "<a=a>Wahoo! It's on!\n<a=c>Dork."),
                    ]),
                ]
            }},
            {"CatMissed", new DialogueMachine(){
                type = NodeType.combat,
                playerShotJustMissed = true,
                allPresent = ["cna"],
                whoDidThat = CnA,
                dialogue = [
                    new([
                        new("cna", "concern", "<a=c>Dang it!\n<a=a>It's okay, babe."),
                        new("cna", "concern", "<a=c>AAAAGH!\n<a=a>You did your best, babe."),
                        new("cna", "concern", "<a=c>Drat!\n<a=a>It's alright. We can always try again."),
                        new("cna", "seriousv2", "<a=c>This! Agh! These controls are useless!\n<a=a>Remember to breathe, babe."),

                        new("cna", "annoyed", "Erk..."),
                        new("cna", "annoyed", "Hrm..."),
                    ]),
                ]
            }.ApplyModData(PairManager.GlobalGirlKey, PairManager.Girl.CAT)},
            {"AmyMissed", new DialogueMachine(){
                type = NodeType.combat,
                playerShotJustMissed = true,
                allPresent = ["cna"],
                whoDidThat = CnA,
                dialogue = [
                    new([
                        new("cna", "annoyed", "<a=a>Uh...\n<a=c>You tried."),
                        new("cna", "annoyed", "<a=a>Erk...\n<a=c>You'll get 'em next time."),
                        new("cna", "annoyedv2", "<a=a>I...\n<a=c>-Did your best."),

                        new("cna", "annoyed", "Erk..."),
                        new("cna", "annoyed", "Hrm..."),
                    ]),
                ]
            }.ApplyModData(PairManager.GlobalGirlKey, PairManager.Girl.AMY)},
            {"CatAndAmyGotHurtButNotTooBad", new DialogueMachine(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                allPresent = ["cna"],
                minDamageDealtToPlayerThisTurn = 1,
                maxDamageDealtToPlayerThisTurn = 1,
                dialogue = [
                    new([
                        new("cna", "neutralv2", "<a=c>That wasn't so bad, right?\n<a=a>Right!"),
                        new("cna", "<a=a>No worse than my hormone injections!\n<a=c>That's the spirit!"),
                        new("cna", "smug", "<a=a>Ha! Is that all you've got?\n<a=c>Pathetic!"),
                        new("cna", "smug", "<a=c>C'mon! Try harder!\n<a=a>Yeah!"),
                        new("cna", "smug", "<a=c>Ha! Just a scratch!"),
                        new("cna", "No worse for wear."),
                        new("cna", "That wasn't too bad."),
                        new("cna", "Coulda been worse!"),
                        new("cna", "smug", "Weak!"),
                    ]),
                ]
            }},
            {"ThatsALotOfDamageToCatAndAmy", new DialogueMachine(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                allPresent = ["cna"],
                minDamageDealtToPlayerThisTurn = 3,
                dialogue = [
                    new([
                        new("cna", "panic", "<a=c>Uh...\n<a=a>We're gonna be okay, right?"),
                        new("cna", "surprised", "<a=a>That. That hurt.\n<a=c>Yeah."),
                        new("cna", "panic", "<a=c>Uh, should the ship be making those sounds?\n<a=a>I sure hope so..."),
                        new("cna", "panic", "<a=a>That wasn't supposed to happen!\n<a=c>Tell me about it!"),
                        new("cna", "serious", "<a=c>Ow! You jerk! You'll pay!\n<a=a>With interest!"),
                        new("cna", "pain", "<a=c>Owowow! That jostled my back injury!\n<a=a>I'll make them pay!!!"),
                        new("cna", "painv2", "<a=c>Agh!\n<a=a>Cat! Are you alright?"),
                        new("cna", "surprised", "<a=a>Eep!\n<a=c>In any other circumstance I'd find that sound incredibly cute."),
                    ]),
                ]
            }},
            {"DizzyWentMissing_CatAndAmy", new DialogueMachine(){
                type = NodeType.combat,
                priority = true,
                oncePerRun = true,
                lastTurnPlayerStatuses = [
                    Status.missingDizzy
                ],
                allPresent = ["cna"],
                oncePerCombatTags = [
                    "dizzyWentMissing"
                ],
                dialogue = [
                    new("cna", "panic", "Uuuuuuh, what happened? Where'd he go?"),
                ]
            }},
            {"RiggsWentMissing_CatAndAmy", new DialogueMachine(){
                type = NodeType.combat,
                priority = true,
                oncePerRun = true,
                lastTurnPlayerStatuses = [
                    Status.missingRiggs
                ],
                allPresent = ["cna"],
                oncePerCombatTags = [
                    "riggsWentMissing"
                ],
                dialogue = [
                    new("cna", "panic", "Ack! What happened to her?!"),
                ]
            }},
            {"PeriWentMissing_CatAndAmy", new DialogueMachine(){
                type = NodeType.combat,
                priority = true,
                oncePerRun = true,
                lastTurnPlayerStatuses = [
                    Status.missingPeri
                ],
                allPresent = ["cna"],
                oncePerCombatTags = [
                    "periWentMissing"
                ],
                dialogue = [
                    new("cna", "panic", "Oh geez! Where'd she go? Is she okay?"),
                ]
            }},
            {"DrakeWentMissing_CatAndAmy", new DialogueMachine(){
                type = NodeType.combat,
                priority = true,
                oncePerRun = true,
                lastTurnPlayerStatuses = [
                    Status.missingDrake
                ],
                allPresent = ["cna"],
                oncePerCombatTags = [
                    "drakeWentMissing"
                ],
                dialogue = [
                    new("cna", "panic", "<a=a>Ack! She's gone! We're not next are we?\n<a=c>We'll be fine, babe. Hopefully."),
                ]
            }},
            {"BooksWentMissing_CatAndAmy", new DialogueMachine(){
                type = NodeType.combat,
                priority = true,
                oncePerRun = true,
                lastTurnPlayerStatuses = [
                    Status.missingBooks
                ],
                allPresent = ["cna"],
                oncePerCombatTags = [
                    "booksWentMissing"
                ],
                dialogue = [
                    new("cna", "panic", "Yikes! That's spooky. She's coming back, right?"),
                ]
            }},
            {"CatAndAmy_ReturnedFromMissing", new DialogueMachine(){
                type = NodeType.combat,
                priority = true,
                oncePerRun = true,
                lastTurnPlayerStatuses = [
                    Status.missingBooks
                ],
                allPresent = ["cna"],
                oncePerCombatTags = [
                    "booksWentMissing"
                ],
                dialogue = [
                    new("cna", "panic", "Yikes! That's spooky. She's coming back, right?"),
                ]
            }.ApplyModData(PairManager.JustReturnedFromMissingKey, true)},
            {"WeJustGainedHeatAndDrakeIsHere_Multi_0", new(){
                edit = [
                    new(EMod.countFromStart, 0, "cna", "squint", "Sweaty."),
                    new(EMod.countFromStart, 0, "cna", "annoyed", "Whoever's responsible for the sudden temperature rise, you'd better be keeping an eye on it!"),
                    new(EMod.countFromStart, 0, "cna", "annoyed", "<a=c>I'm sweating my butt off."),
                ]
            }},
            {"CatAndAmy_GainedHeat", new(){
                type = NodeType.combat,
                lastTurnPlayerStatuses = [
                    Status.heat
                ],
                allPresent = ["cna"],
                oncePerCombatTags = [
                    "DrakeCanYouDoSomethingAboutTheHeatPlease"
                ],
                dialogue = [
                    new([
                        new("cna", "agreeing", "<a=a>Is it getting hot in here or is it just you?\n<a=c>It's you."),
                        new("cna", "squint", "<a=a>I'm sweating. Should I be sweating?\n<a=c>It's like a sauna in here."),
                        new("cna", "agreeing", "<a=c>Amy, you look especially hot today.\n<a=c>I literally am. Is this heat normal?"),
                        new("cna", "annoyedv2", "<a=a>Feels like I just finished dancing for two hours straight.\n<a=c>I'm sweating my butt off."),
                        new("cna", "annoyedv2", "<a=c>Feels like I just finished a two mile run.\n<a=a>Seriously."),
                        new("cna", "squint", "<a=a>Should the consoles feel this warm to touch?\n<a=c>They weren't like this before."),
                        new("cna", "annoyedv2", "<a=a>I don't know what'll overheat first - me or the ship.\n<a=c>Hopefully neither."),
                    ]),
                ]
            }},
            {"CatAndAmy_GainedHeat_Drake_0", new(){
                type = NodeType.combat,
                lastTurnPlayerStatuses = [  
                    Status.heat
                ],
                allPresent = [ "cna", "drake" ],
                oncePerCombatTags = [
                    "DrakeCanYouDoSomethingAboutTheHeatPlease"
                ],
                dialogue = [
                    new("cna", "annoyed", "<a=c>Drake! If you don't keep this heat under control I'll send Amy after you!"),
                    new("drake", "sly", "Oh no, whatever will I do?"),
                ]
            }},
            {"CatAndAmy_GainedHeat_Drake_1", new(){
                type = NodeType.combat,
                lastTurnPlayerStatuses = [  
                    Status.heat
                ],
                allPresent = [ "cna", "drake" ],
                oncePerCombatTags = [
                    "DrakeCanYouDoSomethingAboutTheHeatPlease"
                ],
                dialogue = [
                    new("cna", "annoyed", "<a=a>Drake, I know you think we're hot, but come on."),
                    new("drake", "blush", "Sh-shut up!"),
                ]
            }},
            {"CatAndAmy_GainedHeat_Drake_2", new(){
                type = NodeType.combat,
                lastTurnPlayerStatuses = [  
                    Status.heat
                ],
                allPresent = [ "cna", "drake" ],
                oncePerCombatTags = [
                    "DrakeCanYouDoSomethingAboutTheHeatPlease"
                ],
                dialogue = [
                    new("cna", "annoyed", "<a=a>Drake, if this heat becomes a problem I will put you in a choke hold."),
                    new("drake", "sly", "Is that a threat or a promise?"),
                ]
            }},
            {"CatAndAmy_AboutToDieAndLoop", new(){
                type = NodeType.combat,
                enemyShotJustHit = true,
                maxHull = 2,
                allPresent = [ "cna" ],
                oncePerCombatTags = [
                    "aboutToDie"
                ],
                oncePerRun = true,
                dialogue = [
                    new([
                        new("cna", "concernv2", "<a=a>Cat, if this is it, I love you.\n<a=c>We're not done yet!!!"),
                        new("cna", "concernv2", "<a=a>I'll always be with you, Cat.\n<a=c>Don't give up yet! We're still here!"),
                        new("cna", "concernv2", "<a=a>I don't want to forget.\n<a=c>We won't! No matter what!"),
                        new("cna", "seriousv2", "<a=c>Don't give up Amy! We've got this!"),
                        new("cna", "concernv2", "<a=a>Cat...\n<a=c>Amy, now's not the time to give up!"),
                        new("cna", "concernv2", "<a=a>I love you, Cat.\n<a=c>We're still in it! Don't give up!"),
                        new("cna", "concernv2", "<a=a>I want to hold you, Cat.\n<a=c>Don't give up Amy! We can still fight!"),
                    ])
                ]
            }},
        });

        if (ModEntry.Instance.Helper.ModRegistry.LoadedMods.ContainsKey("Mezz.TwosCompany")) {
            LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>()
            {
                {"mezz_Ilya_WeJustGainedHeat_0", new(){
                    edit = [
                        new(EMod.countFromStart, 0, "cna", "annoyed", "Whoever's responsible for the sudden temperature rise, you'd better be keeping an eye on it!"),
                        new(EMod.countFromStart, 0, "cna", "squint", "<a=a>Should the consoles feel this warm to touch?"),
                        new(EMod.countFromStart, 0, "cna", "annoyed", "<a=c>I'm sweating my butt off."),
                    ]
                }}
            });
        }
    }
}