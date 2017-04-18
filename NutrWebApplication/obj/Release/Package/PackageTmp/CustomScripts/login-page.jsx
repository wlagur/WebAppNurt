class DishesPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = this.props.model;
    }


    enter() {
        var self = this;
        var result = ajaxCall("/home/enter", { name: this.state.Name, password: this.state.Password},
            function (res) {
                self.setState({ messageBox: res });
            }
        );
    }

    changeName(e) {
        this.setState({ Name: e.target.value });
    }

    changePassword(e) {
        this.setState({ Password: e.target.value });
    }

    render() {

        return (
            <div>
                <output>{this.state.messageBox}</output>
                <input value={this.state.Name} onChange={(e) => this.changeName(e)}/>
                <input type="password" value={this.state.Password} onChange={(e) => this.changePassword(e)}/>

                <h1 onClick={() =>this.enter() }>Enter</h1>
            </div>
        );
    }
}

ReactDOM.render(
    <DishesPage model={LoginModel } />,
document.getElementById('root')
);