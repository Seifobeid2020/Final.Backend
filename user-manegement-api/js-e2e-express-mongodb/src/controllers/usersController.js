const userController = (Admin) => {
  // Add user to firebase
  const add = async (req, res) => {
    const { email } = req.body;
    const { password } = req.body;

    const user = await Admin.auth()
      .createUser({
        email,
        password,
      })
      .then((userRecord) => {
        return userRecord.toJSON();
      })
      .catch((error) => {
        console.log(error);
        return false;
      });

    if (user) {
      res.setHeader('Content-Type', 'application/json');
      res.status(201);
      res.send(user);
    } else {
      res.status(500);
      res.send('Error');
    }
  };

  // Get user by uid
  const getById = async (req, res) => {
    const user = await Admin.auth()
      .getUser(req.params.id)
      .then((userRecord) => {
        // See the tables below for the contents of userRecord
        console.log(userRecord);
        return userRecord.toJSON();
      })
      .catch((error) => {
        console.log(error);
        return false;
      });

    if (user) {
      res.status(200);
      res.send(user);
    } else {
      res.status(500);
      res.send('Failed');
    }
  };

  // Update user properties
  const patch = async (req, res) => {
    const uid = req.params.id;

    const params = {};

    Object.keys(req.body).forEach((key) => {
      params[key] = req.body[key];
    });

    const user = await Admin.auth()
      .updateUser(uid, params)
      .then((userRecord) => {
        return userRecord.toJSON();
      })
      .catch((error) => {
        console.log(error);
        return false;
      });

    if (user) {
      res.status(200);
      res.send(user);
    } else {
      res.status(500);
      res.send('Failed');
    }
  };

  // Get user by email
  const getByEmail = async (req, res) => {
    const { email } = req.body;

    const user = await Admin.auth()
      .getUserByEmail(email)
      .then((userRecord) => {
        return userRecord.toJSON();
      })
      .catch((error) => {
        res.status(500);
        console.log(error);
      });

    if (user) {
      res.status(200);
      console.log(user);
      res.send(user);
    }
  };

  // Delete user
  const del = (req, res) => {
    const uid = req.params.id;

    Admin.auth()
      .deleteUser(uid)
      .then(() => {
        res.status(200);
        res.send(`User ${uid} deleted`);
        console.log('User deleted');
      })
      .catch((error) => {
        console.log('Error deleting user: ', error);
        res.status(500);
        res.send('Internal server error');
      });
  };

  // Return the module
  return {
    add,
    getById,
    getByEmail,
    patch,
    del,
  };
};

// Export the module
module.exports = userController;
