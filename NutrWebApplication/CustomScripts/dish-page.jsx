class DishesPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = this.props.model;
        //this.hello = this.hello.bind(this);
        this.state.namesOfDish = Array();
        this.state.selectedDish;
    }

    getDishes() {
        var self = this;

        var result = ajaxCall("/home/students2", { tmp: "i am tmp" },
            function (res) {
                var namesOfDish = JSON.parse(res.json).map(function (key) {
                    var nameOfDish = key.Name;
                    return nameOfDish;
                });
                self.setState({ namesOfDish: namesOfDish });
            }
        );
    }


    changeSelectedDish(e)
    {
        var selectedDish = e.target.value;
        this.setState({ selectedDish: selectedDish });
    }

    render() {
        let studentsContainer =
            <div>
            <select onChange={ (e) => this.changeSelectedDish(e) }>

                {this.state.namesOfDish.map((namesOfDish, index) => {
                    return (
                        <option key={index }>{namesOfDish}</option>);
                })
                }
            </select>
            </div>
        //this.state.selectedDish = studentsContainer.props.children.props.children[0];
        return (
            <div>
                <h1 onClick={() =>this.getDishes() }>Hello, world!</h1>
                <span>{this.state.selectedDish}</span>
                {studentsContainer}
            </div>
        );
    }
}

ReactDOM.render(
    <DishesPage model={DishesModel } />,
document.getElementById('root')
);