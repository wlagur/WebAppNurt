class DishesPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = this.props.model;
        //this.hello = this.hello.bind(this);
        this.state.listAllDishes = Array();
        this.state.selectedDish = {};
        this.state.selectedDish.count;

        this.state.listTables = Array();
        for (var i = 0; i < 5; i++) {
            var table = {
                name: "Table " + (i + 1),
                listDishes: Array()
            };

            this.state.listTables.push(table);
        }

        this.state.listSelectedDishes = this.state.listTables[0].listDishes;
        this.state.currentTable = this.state.listTables[0];

        this.state.messageBox = true;

        this.getDishes();
    }

    getDishes() {
        var self = this;
        var result = ajaxCall("/home/getDishes", { tmp: "i am tmp" },
            function (res) {
                var listAllDishes = JSON.parse(res.json).map(function (key) {
                    var dish = key;
                    dish.count = 1;
                    return dish;
                });

                self.state.selectedDish = listAllDishes[0];
                self.setState({ listAllDishes: listAllDishes });

            }
        );
    }


    changeSelectedDish(e) {
        this.state.selectedDish.count = 1;
        var selectedDish = this.state.listAllDishes[e.target.selectedIndex];
        this.setState({ selectedDish: selectedDish });
    }

    addDish() {
        var self = this;
        var result = ajaxCall("/home/checkToAddDish", { tables: JSON.stringify(this.state.listTables), dish: JSON.stringify(this.state.selectedDish) },
            function (res) {
                if (res == true) {
                    var index = self.state.listSelectedDishes.findIndex((d) => d.Id == self.state.selectedDish.Id);
                    if (index >= 0) { self.state.listSelectedDishes[index].count += self.state.selectedDish.count; }
                    else { self.state.listSelectedDishes.push(jQuery.extend(true, {}, self.state.selectedDish)); }
                    self.state.messageBox = true;
                    self.setState({ listSelectedDishes: self.state.listSelectedDishes });
                }
                else {
                    //self.state.messageBox = res;
                    self.setState({ messageBox: res });
                }
            }

        );

    }

    deleteDish(e) {
        this.state.listSelectedDishes.splice(e.target.id, 1);
        this.setState({ listSelectedDishes: this.state.listSelectedDishes });
    }

    changeCountOfDish(e) {
        this.state.selectedDish.count = parseFloat(e.target.value, 10);
        this.setState({ selectedDish: this.state.selectedDish });
    }

    changeCurrentTable(e) {
        this.state.currentTable = this.state.listTables[e.target.id];
        this.state.listSelectedDishes = this.state.listTables[e.target.id].listDishes;
        this.setState({ listSelectedDishes: this.state.listSelectedDishes });
    }

    closeBill() {
        this.printBill();
        var self = this;
        var result = ajaxCall("/home/closeBill", { tables: JSON.stringify(this.state.listSelectedDishes) },
            function (res) {
                if (res == true) {
                    self.state.listSelectedDishes.splice(0, self.state.listSelectedDishes.length);
                    self.setState({ listSelectedDishes: self.state.listSelectedDishes });
                }
                else {
                    //self.state.messageBox = res;
                    self.setState({ messageBox: res });
                }
            }
        );
    }

    printBill() {
        var self = this;
        var result = ajaxCall("/home/printBill", { tables: JSON.stringify(this.state.listSelectedDishes) },
            function (res) {
                self.setState({ messageBox: res });
            }
        );
    }

    logoff() {
        var self = this;
        var result = ajaxCall("/home/logoff", {},
            function (res) {
                res;
            }
        );
    }

    render() {

        let dishesContainer =
            <div>
            <select onChange={ (e) => this.changeSelectedDish(e) }>

                {this.state.listAllDishes.map((dish, index) => {
                    return (
                        <option key={index} id={index} value={dish}>{dish.Name}</option>);
                })
                }
            </select>
            </div>
        let allPrice = 0;
        let selectedDishesContainer =
            <div>
            <table>
                <thead>
                <tr>
                    <th>Блюдо</th>
                    <th>Цена</th>
                    <th>Количество</th>
                    <th>Сумма</th>
                    <th>Удалить</th>
                </tr>
                </thead>
                <tbody>
                    {this.state.listSelectedDishes.map((dish, index) => {
                    allPrice += dish.Price * dish.count;
                    return (
                        <tr key={index}>
                            <td>{dish.Name}</td>
                            <td>{dish.Price}</td>
                            <td>{dish.count}</td>
                            <td>{dish.Price * dish.count}</td>
                            <td><h3 id={index} onClick={(e) =>this.deleteDish(e)}>удалить</h3></td>
                        </tr>
                    )
                })
                    }
                <tr>
                    <td colSpan="3">Всего:</td>
                    <td colSpan="2">{allPrice}</td>
                </tr>
                </tbody>
            </table>
            </div>
        let listTablesContainet =
            <div>
            <ol>

                {this.state.listTables.map((table, index) => {
                    if (table == this.state.currentTable) {
                        return (

                    <li key={index }><h2 id={index} onClick={(e) =>this.changeCurrentTable(e) }>{table.name}</h2></li>);
                    }
                    else { return (
                    <li key={index }><h4 id={index} onClick={(e) =>this.changeCurrentTable(e) }>{table.name}</h4></li>); }
                })
                }
            </ol>
            </div>
        return (
            <div>
                <link href="../Content/MyStyleSheet.css" rel="stylesheet" />
                <h1 onClick={() =>this.getDishes() } CssClass="heading1">Hello, world!</h1>
                <h1 onClick={() =>this.addDish() }>Add dish</h1>
                <output>{this.state.messageBox}</output>
                {dishesContainer}
                <input type="number" step="any" min="0" value={this.state.selectedDish.count} onChange={(e) => this.changeCountOfDish(e)} />
                {listTablesContainet}
                {selectedDishesContainer}
                <h1 onClick={() =>this.closeBill() }>Close Bill</h1>
                <h1 onClick={() =>this.printBill() }>Print Bill</h1>
                <h1 onClick={() =>this.logoff() }>Exit</h1>
            </div>
        );
    }
}

ReactDOM.render(
    <DishesPage model={DishesModel } />,
document.getElementById('root')
);