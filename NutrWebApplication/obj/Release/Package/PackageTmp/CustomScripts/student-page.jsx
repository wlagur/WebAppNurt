class StudentsPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = this.props.model;
        this.hello = this.hello.bind(this);
    }

    hello(e) {
        console.log("hello");
        
        this.state.list[0] = e;
        this.setState({ list: this.state.list });
    }

    hell() {
        console.log("hell");

        var self = this;
        var result = ajaxCall("/home/students1", { tmp: "i am tmp" },
            function (res) {
                self.setState({ res: res });
                
            }
        );
        //self.state.list = self.state.res.json.list;
        //self.setState({ list: self.state.list });
    }

    render() {
        let studentsContainer = 
            <div> 
                {this.props.model.list.map((student,index) =>
                    {
                    return(
                        <div key={index}><span>{student}</span></div>);
                    })  
                }         
            </div>

        return (
            <div>
                <SimpleText />
                <SimpleText />
                
                <h1 onClick={()=>this.hello("vvvv") }>Hello, world!</h1>
                <h1 onClick={()=>this.hell() }>Hello, hell!</h1>
                {studentsContainer}
            </div>
        );
    }
}

class SimpleText extends React.Component {
    render() {
        return(
            <span>I am just a text</span>);
    }
}

ReactDOM.render(
    <StudentsPage model={StudentsModel }/>,
document.getElementById('root')
);