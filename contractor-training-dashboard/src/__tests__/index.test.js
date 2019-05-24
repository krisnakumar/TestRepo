/** For testing root level components we need to use snapshot testing  */
import Index from '../index.jsx';

it('renders without crashing', () => {
  expect(JSON.stringify(
    Object.assign({}, Index, { _reactInternalInstance: 'censored' })
  )).toMatchSnapshot();
});