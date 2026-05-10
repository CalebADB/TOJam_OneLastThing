
->TheBag

=== TheBag ===
#Pers: Alex
Hey where did your mom get that bag from? #SitMinReq
 + [Austria 3 # Pers: Andy #SichMinReq: V_AndyAnger_2]
    -> MomComplaints
 + [Austria 2 # Pers: Andy #SichMinReq: V_AndyAnger_1]
    # Pers: Andy
    Yeah, the one from austria?
    # Pers: Alex
    Uh, yeah. Well it's not Gucci.
    Had a bad time at the pawn shop today.
    # Pers: Andy
    Mmhmm.
 + [Austria 1 # Pers: Andy]
    # Pers: Andy
    Yeah she got the bag at a christmas market pop-up.
    How much did you get for it? 
    # Pers: Alex
    Ah. #BufferTime: 3 #SichInc: V_AlexAnger_0.3
    # Pers: Andy
    What's wrong? 3 #SichInc: V_AndyFear_0.3
    # Pers: Alex
    I'm like 90 % sure it was fake.
    I took it to the pawn shop and they said $10.
 - Well I'm kinda fucked now. # Pers: Alex 

->TheContract
 
=== MomComplaints ===
# Pers: Andy
Could we not bring my mom into this?
What's up with the bag?
# Pers: Alex
It's a fake.
I wasted a whole 4 hours 
# Pers: Andy
Well it's not a big deal. 
It's not like you bought it.
+ [I don't like her mom #Pers: Alex]
    #Pers: Alex
    You gotta admit that your mom does this a lot. 
    She comes up with stories.
    And she's cool and all but...
    It's still my time.
    I didn't have that type of time today.
    #Pers: Andy
    You gotta lay off my mom.
    #Pers: Alex
    I know, I just feel fucked.
    -> TheContract
+ [It's about the bag not your mom #Pers: Alex]
    #Pers: Alex
    It's not about your mom.
    I was just trying to specify the bag.
    #Pers: Andy
    Okay fine.
    #Pers: Alex
    I'm freaking out.
    -> TheContract
+ [This motha fucka #Pers: Andy #SichMinReq: V_AndyAnger_4]
    #Pers: Andy
    Holy shit dude. This is not that time. 
    Why are you bringing this up now.
    #Pers: Alex
    I wasted so much time today because of your mom.
    As usual.
    #Pers: Andy
    As usual?
    #Pers: Alex
    Yeah, she lies a lot, I thought you were getting better at seeing that.
    #Pers: Andy
    Get to the point.
    -> TheContract
    

=== TheContract ===
# Pers: Andy
What's wrong?
+ [My contractor said I overbilled #Pers: Alex]
    #Pers: Alex
    Okay, so that editing work I was doing for BigTimeNoUnion, 
    they're saying that I overlogged hours. 
    They're not recognizing my invoice.
    #Pers: Andy
    How many hours did you bill them for?
    #Pers: Alex
    300 hours.
    #Pers: Andy
    Dude.
    #Pers: Alex
    Well I've just had so much going on.
    I hate asking for money.
    -> TheRequest
+ [I'm still waiting on my editing money. #Pers: Alex]
    #Pers: Alex
    BigTimeNoUnion hasn't been payed yet.
    #Pers: Andy
    -> TheRequest
+ [This motha fucka #Pers: Andy #SichMinReq: V_AndyAnger_4]
    #Pers: Andy
    Holy shit dude. This is not that time. 
    Why are you bringing this up now.
    #Pers: Alex
    I wasted so much time today because of your mom.
    As usual.
    #Pers: Andy
    As usual?
    #Pers: Alex
    Yeah, she lies a lot, I thought you were getting better at seeing that.
    #Pers: Andy
    Get to the point.
    -> TheRequest
    
-> TheRequest

=== TheRequest ===
#Pers: Alex
I'm not going to be able to pay rent.


-> DONE
 
