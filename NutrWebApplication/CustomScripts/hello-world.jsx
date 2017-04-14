class ReactComp extends React.Component {
    constructor(props) {
        super(props);

        this.hello = this.hello.bind(this);
    }

    hello() {
        console.log("hello");
    }

    render() {
        return (
            <h1 onClick={this.hello }>Hello, world!</h1>
        );
    }
}


ReactDOM.render(
    <ReactComp />,
document.getElementById('helloWorld')
);