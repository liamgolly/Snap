extends Button


var g: game

func simulate(player_one_chance):
	var gg: int
	var royal_wins = []
	var snap_wins = []
	var total_hands = []
	visible = false
	g = get_parent()
	
	g.clear_deck()
	g.populate_deck()
	g.shuffle_deck(g.deck)
	g.deal()
	while(true):

		var time = randf_range(0.25, 0.5)
		#await get_tree().create_timer(time).timeout
		# avg 3.6
		#print('-----------------')
		#print('PLAY CARD - PLAYER ' + str(g.turn))
		
		var played = g.play().split(' | ')
		var card = played[0]
		var royaldeath = len(played) > 1
		#total_hands.append(card)
		#print('CARD: ' + card)
		
		if royaldeath:
			#print('ROYAL DEATH.')
			#royal_wins.append(g.pile.duplicate(true))
			g.win_hand(1 if g.turn == 2 else 2)
			gg = g.game_over()
			#g.shuffle_deck(g.player_one_hand)
			#g.shuffle_deck(g.player_two_hand)

			if gg > 0:
				#print('GG')
				break

		elif g.can_snap():
			var winner = randf_range(0, 1)
			winner = 1 if winner <= player_one_chance else 2
			#print('PLAYER ' + str(winner) + ' SNAPPED')
			#snap_wins.append(g.pile.duplicate(true))
			g.win_hand(winner)
			#g.shuffle_deck(g.player_one_hand)
			#g.shuffle_deck(g.player_two_hand)

			gg = g.game_over()
			if gg > 0:
				#print('GG')
				break
		
		#if g.first_royal:
			#print('ROYAL REMAINING: ' + str(g.active_royal))
		#print('PLAYER 1 CARD COUNT: ' + str(len(g.player_one_hand)))
		#print('PLAYER 2 CARD COUNT: ' + str(len(g.player_two_hand)))
		#print('-----------------')
	
	return gg

func _pressed():
	pass
	
func multi_sim():
	var all_results = []
	for chance in range(100):
		print('============= Running ' + str(chance) + '% =============')
		var player_one_wins = 0
		var player_two_wins = 0
		for i in range(1000):
			if i % 100 == 0:
				print(str(chance) + ": " + str(i) + "/1000")
			var result = simulate(chance / float(100))
			if result == 1:
				player_one_wins += 1
			else:
				player_two_wins += 1
		
		all_results.append(float(player_one_wins) / float(player_one_wins + player_two_wins))
	
	var str_result: String = "Snap Chance, Win Percent\r\n"
	var i: int = 0
	for r in all_results:
		str_result += str(float(i) / float(len(all_results))) + ", " + str(r) + "\r\n"
		i += 1
	print(str_result)

