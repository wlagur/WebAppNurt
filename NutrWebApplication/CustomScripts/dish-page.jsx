class DishesPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = this.props.model;
        //this.hello = this.hello.bind(this);
        this.state.namesOfDish = Array();
        this.state.selectedDish;
        this.state.selectedDishes = Array();
    }

    getDishes() {
        var self = this;

        var result = ajaxCall("/home/students2", { tmp: "i am tmp" },
            function (res) {
                var namesOfDish = JSON.parse(res.json).map(function (key) {
                    var nameOfDish = key.Name;
                    return nameOfDish;
                });

                self.state.selectedDish = namesOfDish[0];
                self.setState({ namesOfDish: namesOfDish });

            }
        );
    }


    changeSelectedDish(e) {
        var selectedDish = e.target.value;
        this.setState({ selectedDish: selectedDish });
    }

    addDish() {
        this.state.selectedDishes.push(this.state.selectedDish);
        this.setState({ selectedDishes: this.state.selectedDishes });
    }

    render() {
        let dishesContainer =
            <div>
            <select onChange={ (e) => this.changeSelectedDish(e) }>

                {this.state.namesOfDish.map((namesOfDish, index) => {
                    return (
                        <option key={index }>{namesOfDish}</option>);
                })
                }
            </select>
            </div>
        let selectedDishesContainer =
            <div>
            <ol>

                {this.state.selectedDishes.map((namesOfDish, index) => {
                    return (
                        <li key={index }>{namesOfDish}</li>);
                })
                }
            </ol>
            </div>
        return (
            <div>
                <h1 onClick={() =>this.getDishes() }>Hello, world!</h1>
                <h1 onClick={() =>this.addDish() }>Add dish</h1>
                {dishesContainer}
                {selectedDishesContainer}
            </div>
        );
    }
}

ReactDOM.render(
    <DishesPage model={DishesModel } />,
document.getElementById('root')
);