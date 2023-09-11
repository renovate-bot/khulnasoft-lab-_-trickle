import {
  ApiOutlined,
  DollarOutlined,
  GithubOutlined,
  IeOutlined,
  TwitterOutlined,
} from "@ant-design/icons";
import { Layout, Tag } from "antd";
import {
  BrowserRouter as Router,
  Link,
  Route,
  RouteComponentProps,
  Switch,
} from "react-router-dom";
import { ListsTable } from "./components";

const { Header, Content, Footer } = Layout;

export const App: React.FC = () => (
  <Router>
    <Layout>
      <Header style={{ background: "#000" }}>
        <Logo />
      </Header>
      <Content>
        <div
          style={{
            paddingLeft: 4,
            paddingRight: 4,
            minHeight: 280,
          }}
        >
          <Switch>
            <Route exact path="/" component={ListsTable} />
            <Route path="/lists/*" component={ListsTable} />
            <Route path="*" component={NotFound} />
          </Switch>
        </div>
      </Content>
      <Footer
        style={{
          textAlign: "center",
          padding: "24px 0px 24px",
          lineHeight: "24px",
        }}
      >
        <CopyrightAuthor />
        <Twitter />
        <GitHub />
        <Api />
        <Tpl />
        <Donate />
      </Footer>
    </Layout>
  </Router>
);

const Logo = () => (
  <Link to="/">
    <img
      src={`${process.env.PUBLIC_URL}/logo_trickle.png`}
      alt="Trickle logo"
      height="44px"
    />
  </Link>
);

const NotFound = (props: RouteComponentProps) => (
  <h2>
    404 Not Found: <code>{props.location.pathname}</code>
  </h2>
);

const CopyrightAuthor = () => (
  <Tag>
    ©{new Date().getFullYear()}&nbsp;
    <a
      href="https://khulnasoft.com"
      title="Collin M. Barrett's Homepage"
      target="_blank"
      rel="noopener noreferrer"
    >
      Collin M. Barrett
    </a>
  </Tag>
);

const Twitter = () => (
  <Tag>
    <a
      href="https://twitter.com/khulnasoft"
      title="Trickle on Twitter"
      target="_blank"
      rel="noopener noreferrer"
    >
      <TwitterOutlined /> Twitter
    </a>
  </Tag>
);

const GitHub = () => (
  <Tag>
    <a
      href="https://github.com/khulnasoft-lab/Trickle"
      title="Trickle git repository on GitHub"
      target="_blank"
      rel="noopener noreferrer"
    >
      <GithubOutlined /> GitHub
    </a>
  </Tag>
);

const Api = () => (
  <Tag>
    <a
      href="/api"
      title="Trickle API"
      target="_blank"
      rel="noopener noreferrer"
    >
      <ApiOutlined /> API
    </a>
  </Tag>
);

const Tpl = () => (
  <Tag>
    <a
      href="/tpl.html"
      title="Trickle Tracking Protection Lists (TPL) for Internet Explorer"
    >
      <IeOutlined /> TPL (IE)
    </a>
  </Tag>
);

const Donate = () => (
  <Tag>
    <a
      href="https://github.com/sponsors/khulnasoft-lab"
      title="Sponsor Collin Barrett on GitHub"
      target="_blank"
      rel="noopener noreferrer"
    >
      <DollarOutlined /> Donate
    </a>
  </Tag>
);
