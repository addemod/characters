window.Character = {
	name: "Character",
	template: "#character-card-template",
	props: {
		character: {
			type: Object,
			required: true
		}
	},

	methods: {
		deleteCharacter() {
			this.$emit("deleteCharacter", this.character.Id)
			//$(this.$refs.deleteCharacterModal).modal("hide")
		}
	}
}
