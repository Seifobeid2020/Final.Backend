var React = require('react');

function DataEntryForm() {
  return (
    <div>
      <form action="/" method="post">
        <div>
          <input type="text" name="name" id="name" placeholder="name" />
        </div>
        <div>
          <input
            type="password"
            name="password"
            id="password"
            placeholder="job"
          />
        </div>
        <select name="city" id="city">
          <option value="" selected disabled>
            Choose a city
          </option>
          <option value="Hebron">Hebron</option>
          <option value="Bethlehem">Bethlehem</option>
          <option value="Ramallah">Ramallah</option>
          <option value="Nablus">Nablus</option>
          <option value="Jerusalem">Jerusalem</option>
          <option value="Gaza">Gaza</option>
        </select>
        <input type="submit" />
      </form>
    </div>
  );
}

function DataList({ dataItems }) {
  return (
    <table>
      <thead>
        <th>Name</th>
        <th>City</th>
        <th>
          Delete {dataItems.length > 1 && <a href="/delete?all=true">all</a>}
        </th>
      </thead>
      {dataItems.map((item) => {
        const id = item._id.toString();
        return (
          <tr key={id} id={id}>
            <td>{item.name}</td>
            <td>{item.city}</td>
            <td>
              <a href={'/delete?id=' + id}>X</a>
            </td>
          </tr>
        );
      })}
    </table>
  );
}

/* props = {
        data: [],
        dbStatus: false
    }
*/
function Index(props) {
  return (
    <DefaultLayout>
      {props.dbStatus === false && <div>No database found.</div>}

      {props.dbStatus === true && (
        <div>
          <div>
            <DataEntryForm></DataEntryForm>
          </div>
          {props.data.length > 0 && (
            <div>
              <DataList dataItems={props.data}></DataList>
            </div>
          )}
        </div>
      )}
    </DefaultLayout>
  );
}

function DefaultLayout(props) {
  return (
    <html>
      <body>{props.children}</body>
    </html>
  );
}

module.exports = Index;
