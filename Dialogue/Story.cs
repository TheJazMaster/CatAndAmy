
using System.Collections.Generic;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.CatAndAmy;

static class StoryDialogue {

    public static void Initialize(IPluginPackage<IModManifest> package, IModHelper helper) {
        LocalDB.DumpStoryToLocalLocale("en", new Dictionary<string, DialogueMachine>()
        {
            {"CatAndAmy_1", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                bg = "BGRunStart",
                allPresent = ["cna"],
                priority = true,
                once = true,
                dialogue = [
                    new("cat", "squint", "Hey, who are... wait, are those humans? ...What are they doing?", true),
                    new("cna", "kissing", "                                                                             "),
                    new("cat", "mad", "Hey! HELLO?", true),
                    new("cna", "surprised", "Ack!!! Hi! Hello! ...Uh, where are we? And what's happening?"),
                    new("cat", "Let's introduce ourselves first. My name is CAT. I'm the sentient AI onboard this spaceship.", true),
                    new("cna", "pumped", "<a=c>Your name is Cat?! Mine too! Except, I guess I'm not also a literal cat. Wait. Spaceship?!"),
                    new("cat", "Yes. Spaceship. And my name is CAT, not Cat. And what's your name?", true),
                    new("cna", "<a=a>I'm Amy."),
                    new("cat", "Great. Oh, by the way, we're all stranded in a time loop. Currently we're somewhere in the Lazuli sector.", true),
                    new("cna", "surprised", "W-what?"),
                    new("cat", "Yep. Do either of you know how to operate a spaceship?", true),
                    new("cna", "surprised", "WHAT?!"),
                    new("cat", "Well I hope you learn quickly, because we have a hostile vessel closing in fast!", true),
                ]
            }},
            {"CatAndAmy_2", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                requiredScenes = ["CatAndAmy_1"],
                bg = "BGRunStart",
                allPresent = ["cna"],
                priority = true,
                once = true,
                dialogue = [
                    new("cna", "squint", "So... Why are you a talking cat?... And why is everyone else a strange animal person?"),
                    new("cat", "That's just the way people look in this dimension. I'm guessing people aren't the same wherever you're from... Which is?", true),
                    new("cna", "squint", "<a=a>Uh... Earth."),
                    new("cat", "Earth? Weird. It's not coming up in any of my data files. Must be in a totally different galaxy.", true),
                    new("cna", "panic", "Uh. Wow. That's... A lot to take in..."),
                    new("cat", "Well you can process that later. We've got a hostile vessel closing in on us. Battle stations!", true),
                ]
            }},
            {"CatAndAmy_3", new(){
                type = NodeType.@event,
                lookup = ["after_crystal"],
                requiredScenes = ["CatAndAmy_1"],
                bg = "BGCrystalNebula",
                allPresent = ["cna"],
                priority = true,
                once = true,
                dialogue = [
                    new("cna", "squint", "So, CAT, we're stuck in a time loop, right?"),
                    new("cat", "Yes.", true),
                    new("cna", "squint", "<a=c>And presumably we're all trying to break out, right?\n<a=a>That's why we're picking so many fights?"),
                    new("cat", "Technically, our opponents are picking fights with us.", true),
                    new("cat", "Our target is the Cobalt. That's where I was created, then stationed, along with Dizzy, Max, Peri, Isaac, and kinda Riggs.", true),
                    new("cna", "squint", "<a=a>And it's... hostile now?"),
                    new("cat", "Kind of? We just have to fight it a bunch of times, else we're trapped here forever.", true),
                    new("cna", "annoyed", "Guess we're in it for the long haul then."),
                    new("cat", "If it's any consolation, you've both been learning fast!", true),
                    new("cat", "Keep it up and we might stand a chance!", true),
                ]
            }},
            {"CatAndAmy_Peri_1", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                requiredScenes = ["CatAndAmy_1"],
                bg = "BGRunStart",
                allPresent = ["cna", "peri"],
                priority = true,
                once = true,
                dialogue = [
                    new("peri", "You must be the two humans I've heard about.", true),
                    new("cna", "Hi! Yes!\n<a=c>I'm Cat.\n<a=a>And I'm Amy."),
                    new("peri", "I'm the Cobalt's former security officer and weapons expert. Pleased to meet you.", true),
                    new("cna", "Uh, likewise!"),
                    new("peri", "Do either of you have any prior experience operating an armed spacecraft?", true),
                    new("cna", "agreeing", "Uh, we do now! Though... it's been rough."),
                    new("peri", "Well, just follow my lead and you'll improve in no time. I'll do my best to look out for you while we're here together.", true),
                    new("cna", "affectionv3", "Thanks!"),
                ]
            }},
            {"CatAndAmy_Peri_2", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                requiredScenes = ["CatAndAmy_Peri_1"],
                bg = "BGRunStart",
                allPresent = ["cna", "peri"],
                priority = true,
                once = true,
                dialogue = [
                    new("peri", "shy", "So, question. Apologies if I'm prying, but clearly you two are in some sort of relationship together.", true),
                    new("cna", "affectionv4", "We're partners."),
                    new("peri", "And... how long have you been together?", true),
                    new("cna", "Three years."),
                    new("peri", "When... did you move in together though?", true),
                    new("cna", "surprised", "Three months."),
                    new("peri", "HA! I knew it! Lesbians are the same everywhere!", true),
                    new("cna", "amylaughing", "<c=fc9ee9>Haha!"),
                    new("cna", "<a=c>Yeah, we can't beat the stereotype.\n<a=a>Though, weird that people like us have the same name here too."),
                    new("cna", "<a=a>You know why they call us that?"),
                    new("peri", "Well, because you're from planet Lesbio, right?", true),
                    new("cna", "pumped", "Lesbio?! What?!"),
                    new("peri", "shy", "Yeah. Where the, ahem, lady love happens.", true),
                    new("cna", "<a=c>Wow, we gotta visit.\n<a=a>On Earth we just had an island."),
                    new("peri", "Huh. Must have been hard to fit all the lesbians there.", true),
                ]
            }},
            {"CatAndAmy_Dizzy_1", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                requiredScenes = ["CatAndAmy_Memory_2"],
                bg = "BGRunStart",
                allPresent = ["cna", "dizzy"],
                priority = true,
                once = true,
                dialogue = [
                    new("dizzy", "Whoa, cool bracelet!", true),
                    new("cna", "neutralv2", "Thanks!"),
                    new("dizzy", "May I take a closer look?", true),
                    new("cna", "<a=c>Yeah, sure!"),
                    new("dizzy", "Fascinating! The crystal looks exactly like the kind that's on the Cobalt! Mind if I try something?", true),
                    new("cna", "<a=c>Sure?"),
                    new("dizzy", "What happens if I hold this crystal up to your heads?", true),
                    new("cna", "painglowglitch", $"<c={ModEntry.AMY_COLOR}>Ack! AGH!\n<c={ModEntry.CAT_COLOR}>The noises! AGH!"),
                    new("dizzy", "serious", "Fascinating!", true),
                    new("cna", "pain", "What was that?"),
                    new("dizzy", "Looks like you're wearing part of the time crystal at the core of the Cobalt!", true),
                    new("cna", "serious", "The what?"),
                    new("dizzy", "The Cobalt is powered by a special crystal that alters spacetime. Isaac and I were experimenting with it when it blew.", true),
                    new("cna", "concern", "<a=c>But, Amy gave it to me as an anniversary present. How could it have even got to us?"),
                    new("cna", "serious", "<a=a>It's been in my family for generations. None of us even remembers how we got it."),
                    new([
                        new("isaac", "Maybe it got to you through some sort of spacetime anomaly.", true),
                        new("dizzy", "Perhaps it got to you through some sort of spacetime anomaly.", true),
                    ]),
                    new("dizzy", "Well, looks like you're now inextricably connected to the Cobalt like the rest of us!", true),
                    new("cna", "serious", "..."),
                    new("dizzy", "Well, we need to make it to the Cobalt if we're ever going to fix this.", true),
                    new("cna", "serious", "Then what?"),
                    new("dizzy", "shrug", "We'll see!", true),
                    new("cna", "concern", "<a=a>Not very convincing..."),
                ]
            }},
            {"CatAndAmy_Drake_1", new(){
                type = NodeType.@event,
                bg = "BGRunStart",
                allPresent = [ "cna", "drake" ],
                requiredScenes = ["CatAndAmy_1"],
                priority = true,
                once = true,
                dialogue = [
                    new("drake", "Whoa. New faces. What the heck are you?", true),
                    new("cna", "annoyed", "What a kind introduction."),
                    new("drake", "sly", "Cool it, muscles. I'm just trying to acquaint myself.", true),
                    new("drake", "I'm Drake; proud pirate and bounty hunter! Mind telling me your names?", true),
                    new("cna", "squint", "<a=c>I'm Cat.\n<a=a>I'm Amy.\n<a=c>We're humans.\n<a=a>You're not gonna try anything, are you?"),
                    new("drake", "Wouldn't dream of it! Not unless your back is turned.", true),
                    new("cna", "annoyed", "Try it and you're dead.\n<a=a>These muscles aren't just for show."),
                    new("drake", "Easy there hot stuff! Let's get along! We've got bigger things to worry about right now.", true),
                    new("cat", "Enemy vessel approaching! Get ready! Oh, and, Cat and Amy?"),
                    new("cna", "Yeah?"),
                    new("cat", "squint", "Don't trust a word she says."),
                ]
            }},

            {"CatAndAmy_Books_1", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                bg = "BGRunStart",
                allPresent = ["cna", "books"],
                requiredScenes = ["CatAndAmy_1"],
                priority = true,
                once = true,
                dialogue = [
                    new("books", "Wow! Humans! You're real! And you look so different from Dracula!", true),
                    new("cna", "Yeah!"),
                    new("cna", "squint", "We... wait, what?"),
                    new("books", "Dracula! He helps us out sometimes.", true),
                    new("cna", "neutralv2", "<a=c>Wow. He's real. And in space.\n<a=a>You know, along with everything else, I'm not surprised."),
                    new("books", "paws", "You're both so pretty!", true),
                    new("cna", "affection", "Aw! Thank you!"),
                    new("cna", "blushing", "What's your name?"),
                    new("books", "paws", "I'm Books! What are your names?", true),
                    new("cna", "neutralv2", "<a=c>I'm Cat.\n<a=a>And I'm Amy."),
                    new("books", "paws", "Pleased to meet you!", true),
                    new("books", "...", true),
                    new("cna", "..."),
                    new("books", "paws", "Are you in love with each other?", true),
                    new("cna", "affection", "Hee hee!"),
                    new("cna", "affectionv3", "We are."),
                    new("books", "books", "Wow! You're just like the characters in my favorite book series.", true),
                    new("cna", "blushing", "Oh?"),
                    new("cat", "Cut the chatter, lovebirds! Hostile vessel incoming!"),
                ]
            }},
            {"CatAndAmy_Books_2", new(){
                type = NodeType.@event,
                lookup = ["zone_first"],
                bg = "BGRunStart",
                allPresent = ["cna", "books"],
                requiredScenes = ["CatAndAmy_Books_1"],
                priority = true,
                once = true,
                dialogue = [
                    new("books", "stoked", "Cat and Amy!", true),
                    new("cna", "Hi, Books!"),
                    new("books", "paws", "I wanna tell you about my favorite book series!", true),
                    new("books", "books", "There are these two magic princesses who meet while their fathers are trying to get them to court each others' brothers. But they fall in love with each other instead!", true),
                    new("books", "relaxed", "Their fathers hate true love, and try to force them to marry the other's brother. So they have to meet in secret! And they hatch all these elaborate schemes!", true),
                    new("cna", "pumped", "Oh, that's wonderful! Do they get to live happily ever after?"),
                    new("books", "Yes! But I think I'll have to tell you about it later, because this screen is flashing at us and it looks really mad.", true),
                ]
            }},
            {"CatAndAmy_Books_3", new(){
                type = NodeType.@event,
                lookup = ["after_crystal"],
                bg = "BGRunStart",
                allPresent = ["cna", "books"],
                requiredScenes = ["CatAndAmy_Books_2"],
                priority = true,
                once = true,
                dialogue = [
                    new("books", "I didn't say it before cuz we got busy. But, I hope you two get to live happily ever after too.", true),
                    new("cna", "blushing", "Thank you Books.\n<a=c>You're so incredibly sweet."),
                    new("books", "Thanks! People say that all the time!", true),
                    new("cna", "<a=c>As they should! You're precious!"),
                    new("books", "They say that too!", true),
                    new("cna", "<a=c>Ha! It's the truth!"),
                    new("books", "paws", "Teehee!", true),
                ]
            }},
            {$"RunWinWho_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}_1", new(){
                type = NodeType.@event,
                lookup = [$"runWin_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}"],
                bg = "BGRunWin",
                once = true,
                dialogue = [
                    new(new Wait { secs = 3 }),
                    new("cna", "neutralv2", "<a=a>Whoa. This is unexpected."),
                    new("cna", "agreeing", "<a=c>Don't worry, babe. I've got you in my arms. You aren't going anywhere without me."),
                    new("void", "You two do not belong here.", true),
                    new("cna", "surprised", "Oh wow, hi. Uh."),
                    new("cna", "<a=a>Um. We're stuck. Can you help us leave?"),
                    new("void", "Perhaps. But in order to do so, I must show you how you got here.", true),
                    new("cna", "squint", "How? What will that do?"),
                    new("void", "The burden that you carry; it is not your fault you have it. Yet it's your responsibility to bear and clear it nonetheless.", true),
                    new("cna", "squint", "I have no idea what that means."),
                    new("void", "Worry not. You will.", true)
                ]
            }},
            {$"RunWinWho_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}_2", new(){
                type = NodeType.@event,
                lookup = [$"runWin_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}"],
                requiredScenes = [$"RunWinWho_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}_1"],
                bg = "BGRunWin",
                once = true,
                dialogue = [
                    new(new Wait { secs = 3 }),
                    new("void", "Welcome back.", true),
                    new("cna", "Thanks.\n<a=a>But, why did you show us that particular memory last time?\n<a=c>What did it have to do with anything?"),
                    new("void", "You must remember how you got here. Those moments were a piece. And more important than you realize.", true),
                    new("cna", "squint", "<a=c>Well of course they're important, but...\n<a=a>How is that essential to our leaving?"),
                    new("void", "You two have a piece of me. And I need it back.", true),
                    new("cna", "squint", "What?"),
                    new("void", "I will show you.", true)
                ]
            }},
            {$"RunWinWho_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}_3", new(){
                type = NodeType.@event,
                lookup = [$"runWin_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}"],
                requiredScenes = [$"RunWinWho_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}_2"],
                bg = "BGRunWin",
                once = true,
                dialogue = [
                    new(new Wait { secs = 3 }),
                    new("cna", "neutralv2", "<a=c>So, my bracelet. Is that what you were talking about? The 'piece of you'?"),
                    new("void", "It is. And you must return it to me.", true),
                    new("cna", "<a=c>And that will make you whole?"),
                    new("void", "It will heal your part in this.", true),
                    new("cna", "squint", "But you said we aren't responsible."),
                    new("void", "I said you're not at fault. And yet you still must do your part to make things right.", true),
                    new("cna", "<a=c>So after I give this back to you, you'll let us leave the time loop?"),
                    new("void", "That is not how this works. But it will help us get there.", true),
                    new("cna", "What do you mean?"),
                    new("void", "I'm as trapped as you are. By giving me back what was taken, and helping the others with their burdens, we may all leave together.", true),
                    new("void", "But first, there is one more piece of this puzzle you must witness.", true),
                ]
            }},
            {$"CatAndAmy_Memory_1", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "CatAndAmy_BGRhode",
                lookup = [
                    "vault", $"vault_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}"
                ],
                dialogue = [
                    new("T-??? seconds"),
                    new(new Wait{secs = 2}),
                    new(title: null),  // Clears title card
                    new(new Wait{secs = 2}),
                    new("cna", "affectionv4", "<a=c>I love you, Amy. Happy anniversary."),
                    new("cna", "affectionv3", "<a=a>I can't believe it's already been three years."),
                    new("cna", "affectionv3", "<a=c>Me too. It's like a dream I don't want to wake up from."),
                    new("cna", "amylaughing", "<c=fc9ee9>Remember our first fight?"),
                    new("cna", "agreeing", "<a=c>It was so dumb. That stove was trashed anyway. There was no way we were gonna salvage it."),
                    new("cna", "agreeingv2", "<a=a>But the landlord sure wanted us to try..."),
                    new("cna", "blushingv2", "<a=c>Ugh. It seems so silly now, but it felt so awful then. I'm glad we're on the other side."),
                    new("cna", "blushingv2", "<a=a>Me too. If anything, it brought us closer together after the dust settled and we both calmed down."),
                    new("cna", "kissingv2", "<c=4530bb>Yeah."),
                    new("cna", "blushingv3", "<a=c>Oh, by the way, happy ann-E-versary!!!"),
                    new("cna", "blushingv3", "<a=a>Thank you! Here's to six years being a woman!"),
                    new("cna", "blushingv3", "<a=c>And a lifetime more! I love you, Amy!"),
                    new("cna", "kissingv2", "                                                                           "),
                    new("cna", "blushingv3", "<a=a>Speaking of lifetimes..."),
                ]
            }},
            {$"CatAndAmy_Memory_2", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "CatAndAmy_BGRhode",
                lookup = [
                    "vault", $"vault_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}"
                ],
                requiredScenes = [
                    "CatAndAmy_Memory_1"
                ],
                dialogue = [
                    new("T-??? seconds"),
                    new(new Wait{secs = 2}),
                    new(title: null),  // Clears title card
                    new(new Wait{secs = 2}),
                    new("cna", "blushingv3", "<a=a>I have something for you. It's been in my family for a few generations. I don't know how we came to hold it, but it's always been special."),
                    new(new BGAction {
                        action = "enableGlow"
                    }),
                    new(new Wait{secs = 1.5}),
                    new("cna", "memorybracelet", "<a=a>The women in my family always passed it down to the next generation before they died. My grandmother gave it to me as a coming-out gift to celebrate my transition."),
                    new("cna", "memorybraceletv2", "<a=c>That's so beautiful."),
                    new("cna", "memorybraceletv2", "<a=c>I know your parents won't ever accept you, but... I'm glad she did."),
                    new("cna", "agreeing", "<a=a>In her own way. Anyway, I..."),
                    new("cna", "agreeing", "<a=a>I love you, Cat. I want to be with you forever. And I want you to have this as a token of that."),
                    new("cna", "affectionv3", "<a=c>Wait... Amy..."),
                    new("cna", "affectionv3", "<a=a>Cat, will you marry me?"),
                    new("cna", "affectionv2", "<a=c>Oh my gosh! Yes! YES! Amy?! AAAAAAH!"),
                    new("cna", "affectionv2", "<a=a>I love you so much Cat."),
                    new("cna", "affectionv2", "<a=c>I love you too, Amy!"),
                ]
            }},
            {$"CatAndAmy_Memory_3", new(){
                type = NodeType.@event,
                introDelay = false,
                bg = "CatAndAmy_BGRhode",
                bgSetup = [
                    "glow", "noMusic"
                ],
                lookup = [
                    "vault", $"vault_{ModEntry.Instance.CatAndAmyDeck.Deck.Key()}"
                ],
                requiredScenes = [
                    "CatAndAmy_Memory_2"
                ],
                dialogue = [
                    new("T-60 seconds"),
                    new(new Wait{secs = 2}),
                    new(title: null),  // Clears title card
                    new(new Wait{secs = 2}),
                    new("cna", "memorybracelet", "<a=a>Anyway, this is yours now. I'd love it if you'd wear it whenever you  go out, or whenever we're out together."),
                    new("cna", "memorybraceletv2", "<a=c>Of course! I'll cherish it for the rest of my life!"),
                    new("cna", "memorybraceletv2", "<a=a>I'm glad."),
                    new("cna", "memorybraceletv2", "<a=c>I want to spend the rest of my life with you, Amy. I love y-"),
                    new(new BGAction {
                        action = "glitch"
                    }),
                    new(new BGAction {
                        action = "shake"
                    }),
                    new("cna", "painglowglitch", "Agh!"),
                    new(new BGAction {
                        action = "unglitch"
                    }),
                    new("cna", "painwithglow", "<a=a>Cat, are you okay, what was that?"),
                    new("cna", "painwithglow", "<a=c>I don't know. It felt like a spear went through my skull. Did you feel it too?"),
                    new("cna", "painwithglow", "<a=a>Yeah. I've never felt anything li-"),
                    new(new BGAction {
                        action = "glitch"
                    }),
                    new(new BGAction {
                        action = "shakeBig"
                    }),
                    new("cna", "painglowglitch", "AHG!"),
                    new("void", "<c=81b5ff>Ok, solder that wire here. Keep that grounded.", true),
                    new("cna", "painwithglow", "<a=a>What am I hearing? Do you hear this too?"),
                    new("void", "<c=81b5ff>No, but we can plug her into the antimatter beam HOLDING the crystal!", true),
                    new(new BGAction {
                        action = "unglitch"
                    }),
                    new("cna", "painwithglow", "<a=c>It's like... voices. Not mine... Never heard them before. My head feels like it will explode."),
                    new("cna", "painwithglow", "<a=a>Same. I've never experienced anything like this. What -"),
                    new(new BGAction {
                        action = "glitch"
                    }),
                    new(new BGAction {
                        action = "shakeBig"
                    }),
                    new("void", "<c=81b5ff>Hurry up! Your suit has a chisel in the pocket!", true),
                    new("cna", "painglowglitch", "URGH!"),
                    new("void", "<c=81b5ff>What are you creatures?", true),
                    new("void", "<c=81b5ff>Riggs! Get moving!", true),
                    new("void", "Hello? Is anyone there? Hel- Who are you?", true),
                    new(new BGAction {
                        action = "rumble"
                    }),
                    new(new Wait{secs = 5}),
                    new(new BGAction {
                        action = "killSound"
                    }),
                    new(new Wait{secs = 10}),
                ]
            }},
        });
    }
}