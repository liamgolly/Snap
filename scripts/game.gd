extends Node
class_name game

var suits = ['spade', 'club', 'diamond', 'heart']
var nums = ['ace', '2','3','4','5','6','7','8','9','10', 'jack','queen','king']

var deck : Array[String] = []
var pile : Array[String] = []
var first_royal: bool = false
var active_royal : int = 0
var turn: int = 1

var player_one_hand : Array[String] = []
var player_two_hand : Array[String] = []

func clear_deck():
	deck = []

func populate_deck():
	# Cards are stored as spade|club|diamond|heart {2-10, jack|queen|king|ace}
	for s in suits:
		for n in nums:
			deck.append(s + '_' + n)

func shuffle_deck(toshuffle: Array):
	toshuffle.shuffle()

func deal():
	player_one_hand = deck.slice(0, 26)
	player_two_hand = deck.slice(26, 52)

func play() -> String:
	var card: String
	if (len(player_one_hand) == 0) || (len(player_two_hand) > 0 && turn == 2):
		card = player_two_hand.pop_front()
	else:
		card = player_one_hand.pop_front()

	pile.push_back(card)
	if 'jack' in card or 'queen' in card or 'king' in card or 'ace' in card:
		active_royal = 1 if 'jack' in card else \
					   2 if 'queen' in card else \
					   3 if 'king' in card else 4
		turn = 2 if turn == 1 else 1
		first_royal = true
		return card
	
	if active_royal > 1:
		active_royal -= 1
		return card
	
	if first_royal:
		return card + ' | royaldeath'
		
	turn = 2 if turn == 1 else 1
	return card

func can_snap() -> bool:
	var pile_len = len(pile)
	if pile_len == 0:
		return false
	
	var top = pile[-1]
	
	var second: String = ""
	if pile_len > 1:
		second = pile[-2]
	
	var third: String = ""
	if pile_len > 2:
		third = pile[-3]
	
	var top_num = top.split('_')[1]
	var second_num = '0' if second == "" else second.split('_')[1]
	var third_num = '0' if third == "" else third.split('_')[1]
	
	var num = nums.find(top_num)
	
	if pile_len % 13 == num + 1: # Counting
		return true
	
	if top_num == second_num: # Double
		return true
		
	if top_num == third_num: # Sandwich
		return true
	
	return false

func win_hand(player: int):
	if player == 1:
		player_one_hand.append_array(pile)
	if player == 2:
		player_two_hand.append_array(pile)
	pile = []
	turn = player
	first_royal = false
	active_royal = 0

func game_over() -> int:
	if len(player_one_hand) == 0:
		return 2
	if len(player_two_hand) == 1:
		return 1
	return 0
